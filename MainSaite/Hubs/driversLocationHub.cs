using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MainSaite.Models;

namespace MainSaite.Hubs
{
	[HubName("driverLocationHub")]
    public class DriversLocationHub : Hub
	{
		static ICollection<SignalRUser> driverLocationHubUsers = new List<SignalRUser>();


		//[HubMethodName("sendToOperators")]
		//public void SendToOperators(string message, string userName)
		//{
		//	Clients.Group("Operator").showMessage(message, userName);
		//}


		////Assign order to driver
		//[HubMethodName("assignedOrder")]
		//public void AssignedOrder(OrderDTO order)
		//{
		//	Clients.Group("Operator").assignedDrOrder(order);
		//}

		[HubMethodName("connectUser")]
		public void ConnectUser(int roleId, int userId)
		{
			string connectionId = Context.ConnectionId;
			int RoleId = roleId;
			var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = roleId, UserId = userId };
			if (!driverLocationHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				if (roleId == 1)
				{
					currentUser.Group = "Driver";
					Groups.Add(Context.ConnectionId, "Driver");
				}
				if (roleId == 2)
				{
					currentUser.Group = "Operator";
					Groups.Add(Context.ConnectionId, "Operator");
				}
				driverLocationHubUsers.Add(currentUser);
			}
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = driverLocationHubUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				Clients.All.driverFinish();
				driverLocationHubUsers.Remove(item);
			}
			return base.OnDisconnected(stopCalled);
		}
	}
}