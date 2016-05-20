using BAL.Interfaces;
using MainSaite.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Hubs
{
	[HubName("MainHub")]
	public class MainHub : Hub
	{
		static ICollection<SignalRUserEx> orderHubUsers = new List<SignalRUserEx>();

		static Dictionary<int, List<string>> DistrictMap = new Dictionary<int, List<string>>();

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
		public void OrderApproved(int id)
		{
			Clients.Group("Driver").OrderApproved(id);
			Clients.OthersInGroup("Operator").approveOrder(id);
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
			if (!DistrictMap.Keys.Contains(id))
			{
				DistrictMap.Add(id, new List<string>());
			}
			DistrictMap[id].Add(Context.ConnectionId);
			Clients.Group("Driver").addDriverToDistrict(id);
		}

		[HubMethodName("leaveDistrict")]
		public void LeaveDistrict(int id)
		{
			if (DistrictMap.Keys.Contains(id))
			{
				DistrictMap[id].Remove(Context.ConnectionId);
			}

			Clients.Group("Driver").subtractDriverFromDistrict(id);
		}

		[HubMethodName("getDriversCount")]
		public List<DistrictCount> GetDriversCount()
		{
			return DistrictMap.Select(c => new DistrictCount() { Id = c.Key, Count = c.Value.Count }).ToList();
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var current = DistrictMap.Where(c => c.Value.Contains(Context.ConnectionId)).FirstOrDefault().Key;
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