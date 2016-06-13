using BAL.Interfaces;
using MainSaite.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BAL.Manager;
using Common.Enum.DriverEnum;
using DAL.Interface;
using DAL;
using System.Threading;
using System.Globalization;

namespace MainSaite.Hubs
{
	[HubName("MainHub")]
	public class MainHub : Hub
	{
		static ICollection<SignalRUserEx> orderHubUsers = new List<SignalRUserEx>();

		static Dictionary<int, List<string>> districtMap = new Dictionary<int, List<string>>();

		[HubMethodName("addOrder")]
		public void AddOrder(OrderExDTO order)
		{
			var currentUser = orderHubUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
			currentUser.OrderId = order.Id;
			Clients.Group("Operator").addOrder(order);
		}

		[HubMethodName("cancelOrder")]
		public void CancelOrder(int id)
		{
			Clients.Group("Operator").cancelOrder(id);
			var driver = orderHubUsers.FirstOrDefault(u => u.OrderId == id && u.Group=="Driver");
			if (driver !=null)
			{
				Clients.Client(driver.ConnectionId).cancelOrder(id);
				Clients.Client(driver.ConnectionId).MessageFromAdministrator(Resources.Resource.CancelOrder);
			}
		}

		[HubMethodName("OrderConfirmed")]
		public void OrderConfirmed(int OrderId, int WaitingTime)
		{
			var client = orderHubUsers.FirstOrDefault(u => u.OrderId == OrderId);
			var driver = orderHubUsers.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
			driver.OrderId = OrderId;
			if (client != null)
				Clients.Client(client.ConnectionId).OrderConfirmed(OrderId, WaitingTime);
			Clients.Group("Operator").confirmOrder(OrderId);
		}

		[HubMethodName("OrderApproved")]
		public void OrderApproved(int id, int districtId)
		{
			Clients.OthersInGroup("Operator").approveOrder(id);
			new Task(() => {
				if (districtMap.Keys.Contains(districtId))
				{
					var firstDrivers = districtMap[(int)districtId].Take(6).ToArray();
					foreach (var driver in firstDrivers)
					{
						Clients.Client(driver).OrderApproved(id);
						Task.Delay(10000).Wait();
					}
					Clients.Group("Driver", firstDrivers).OrderApproved(id);
				}
				else
				{
					Clients.Group("Driver").OrderApproved(id);
				}
			}).Start();
			
		}

		[HubMethodName("OrderApproved")]
		public void OrderApproved(int id)
		{
			var client = orderHubUsers.FirstOrDefault(u => u.OrderId == id);
			if (client != null)
				Clients.Client(client.ConnectionId).OrderApproved();
			Clients.Group("Driver").OrderApproved(id);
		}

		[HubMethodName("denyOrder")]
		public void DenyOrder(int id)
		{
			var client = orderHubUsers.FirstOrDefault(u => u.OrderId == id);
			if (client != null)
				Clients.Client(client.ConnectionId).OrderDenied();
			Clients.OthersInGroup("Operator").denyOrder(id);
		}

		[HubMethodName("updateOrder")]
		public void UpdateOrder(int id)
		{
			Clients.Group("Operator").orderUpdated(id);
		}
		[HubMethodName("orderFinished")]
		public void FinishOrder(int id)
		{
			Clients.Group("Operator").finishOrder(id);
		}
		[HubMethodName("MessageFromAdministrator")]
		public void MessageFromAdministrator(String message)
		{
			Clients.Group("Driver").MessageFromAdministrator(message);
		}

		[HubMethodName("notifyDriverCoordinate")]
		public void NotifyDriverCoordinate(CoordinatesExDTO coordinate)
		{
			var client = orderHubUsers.FirstOrDefault(u => u.OrderId == coordinate.OrderId);
			if (client != null)
			{
				Clients.Client(client.ConnectionId).notifyDriverCoordinate(coordinate);
			}
		}

		[HubMethodName("connect")]
		public void Connect(string group)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUserEx() { ConnectionId = connectionId, Group = group };
			if (!orderHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				Groups.Add(Context.ConnectionId, group);
				orderHubUsers.Add(currentUser);
			}
		}

		[HubMethodName("connect")]
		public void Connect(string group, int orderId)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUserEx() { ConnectionId = connectionId, Group = group, OrderId = orderId };
			if (!orderHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				Groups.Add(Context.ConnectionId, group);
				orderHubUsers.Add(currentUser);
			}
		}

		[HubMethodName("connectDriver")]
		public void ConnectDriver(string group, int userId)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUserEx()
			{
				ConnectionId = connectionId,
				Group = group,
				UserId = userId
			};

			if (!orderHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				Groups.Add(Context.ConnectionId, group);
				orderHubUsers.Add(currentUser);
			}
		}

		// methods for districts 

		[HubMethodName("joinDistrict")]
		public void JoinDistrict(int id)
		{
			if (!districtMap.Keys.Contains(id))
			{
				districtMap.Add(id, new List<string>());
			}
			districtMap[id].Add(Context.ConnectionId);
			Clients.Group("Driver").addDriverToDistrict(id);
		}

		[HubMethodName("leaveDistrict")]
		public void LeaveDistrict(int id)
		{
			if (districtMap.Keys.Contains(id))
			{
				districtMap[id].Remove(Context.ConnectionId);
			}

			Clients.Group("Driver").subtractDriverFromDistrict(id);
		}

		[HubMethodName("getDriversCount")]
		public List<DistrictCount> GetDriversCount()
		{
			return districtMap.Select(c => new DistrictCount() { Id = c.Key, Count = c.Value.Count }).ToList();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			var current = districtMap.Where(c => c.Value.Contains(Context.ConnectionId)).FirstOrDefault().Key;
			LeaveDistrict(current);
			var item = orderHubUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				orderHubUsers.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}

		[HubMethodName("blockDriver")]
		public void BlockDriver(int driverId, string message, string whileTime, string untilTime, string nowTime)
		{
			IUnitOfWork uOW = new UnitOfWork();
			IWorkerStatusManager workerStatusManager = new WorkerStatusManager(uOW);
			IUserManager userManager = new UserManager(uOW);

			TimeSpan? timeSpan = null;
			DateTime? blockTime = null;
			var now = DateTime.Parse(nowTime);

			if (whileTime != null)
			{
				var time = whileTime.Split(':');
				timeSpan = new TimeSpan(int.Parse(time[0]), int.Parse(time[1]), 0);
				blockTime = now.Add(timeSpan.Value);
			}
			if (untilTime != null)
			{
				var time = untilTime.Replace(' ', '-').Replace(':', '-').Split('-');
				blockTime = new DateTime(int.Parse(time[2]), int.Parse(time[1]), int.Parse(time[0]), int.Parse(time[3]), int.Parse(time[4]), 0);
				timeSpan = (TimeSpan)(blockTime - now);
			}

			var driver = userManager.GetById(driverId);
			workerStatusManager.ChangeStatus(driver, DriverWorkingStatusEnum.Blocked, blockTime, message);

			var client = orderHubUsers.FirstOrDefault(u => u.UserId == driverId);
			if (client != null)
				Clients.Client(client.ConnectionId).blockDriver(blockTime, message);

			Clients.Client(Context.ConnectionId).blockDriver(blockTime, driverId);
			Clients.OthersInGroup("Operator").blockDriver(blockTime, driverId);

			if (timeSpan.HasValue)
				UnblockDriverDelay(timeSpan.Value, driverId);
		}

		[HubMethodName("unblockDriver")]
		public void UnblockDriver(int driverId)
		{
			IUnitOfWork uOW = new UnitOfWork();
			IWorkerStatusManager workerStatusManager = new WorkerStatusManager(uOW);
			workerStatusManager.DeleteStatus(driverId);

			var client = orderHubUsers.FirstOrDefault(u => u.UserId == driverId);
			if (client != null)
				Clients.Client(client.ConnectionId).unblockDriver();

			Clients.Client(Context.ConnectionId).unblockDriver(driverId);
			Clients.OthersInGroup("Operator").unblockDriver(driverId);
		}

		private void UnblockDriverDelay(TimeSpan time, int driverId)
		{
			var t = Task.Factory.StartNew(() =>
			{
				Task.Delay(time).Wait();
				UnblockDriver(driverId);
			});
			t.Wait();
		} 
	}
}