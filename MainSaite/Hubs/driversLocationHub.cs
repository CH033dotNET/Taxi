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

		[HubMethodName("updateDriverPosition")]
		public void updateDriverPosition(int id, double lat, double lng, DateTime startTime, string name)
		{
			Clients.Group("Operator").locationUpdate(lat, lng, DateTime.Now, startTime, id, name);
		}

		[HubMethodName("connectUser")]
		public void ConnectUser(string role)
		{
			string connectionId = Context.ConnectionId;

				if (role == "Driver"){
					Groups.Add(Context.ConnectionId, "Driver");
				}
				if (role == "Operator"){
					Groups.Add(Context.ConnectionId, "Operator");
				}
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
		
				Clients.All.driverFinish();
				//driverLocationHubUsers.Remove(item);
			return base.OnDisconnected(stopCalled);
		}
	}
}