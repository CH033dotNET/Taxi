using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Model.DTO;
using MainSaite.Models;
namespace MainSaite.Hubs
{
	[HubName("AdminHub")]
	public class AdminHub : Hub
	{
		static ICollection<SignalRUser> AdminHubUsers = new List<SignalRUser>();

		[HubMethodName("MessageFromAdministrator")]
		public void MessageFromAdministrator(String message)
		{
			var context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();
			context.Clients.Group("Driver").MessageFromAdministrator(message);
		}
	

		[HubMethodName("connect")]
		public void Connect(string group) {
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUser() { ConnectionId = connectionId, Group = group };
			if (!AdminHubUsers.Any(x => x.ConnectionId == connectionId)) {
				Groups.Add(Context.ConnectionId, group);
				AdminHubUsers.Add(currentUser);
			}
		}

	public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = AdminHubUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				AdminHubUsers.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}