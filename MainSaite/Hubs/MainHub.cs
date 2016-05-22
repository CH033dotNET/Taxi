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


		[HubMethodName("OrderConfirmed")]
		public void OrderConfirmed(int OrderId, int WaitingTime)
		{
			var client = orderHubUsers.FirstOrDefault(u => u.OrderId == OrderId);
			if (client != null)
			{
				Clients.Client(client.ConnectionId).OrderConfirmed(WaitingTime);
			}
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
			Clients.Group("Driver").OrderApproved(id);
		}

		[HubMethodName("denyOrder")]
		public void DenyOrder(int id)
		{
			Clients.OthersInGroup("Operator").denyOrder(id);
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

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
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
	}
}