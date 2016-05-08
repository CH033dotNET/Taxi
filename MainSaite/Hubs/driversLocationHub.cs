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

		[HubMethodName("updateDriverPosition")]
		public void updateDriverPosition(int id, double lat, double lng, string name)
		{
			Clients.Group("Operator").locationUpdate(lat, lng, DateTime.Now, id, name);
		}

		[HubMethodName("connectUser")]
		public void ConnectUser(string role, int id)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUser() { ConnectionId = connectionId, UserId = id };
			if (!driverLocationHubUsers.Any(x => x.UserId == id))
			{
				if (role == "Driver"){
					Groups.Add(Context.ConnectionId, "Driver");
				}
				if (role == "Operator"){
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
				Clients.Group("Operator").driverFinish(item.UserId);
				driverLocationHubUsers.Remove(item);
			}
				//driverLocationHubUsers.Remove(item);
			return base.OnDisconnected(stopCalled);
		}
	}
}