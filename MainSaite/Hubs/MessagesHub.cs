using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MainSaite.Models;
using MainSaite.Models;
namespace MainSaite.Hubs
{
	public class MessagesHub : Hub
	{
		static ICollection<SignalRUser> Users = new List<SignalRUser>();
		static ICollection<SignalRGroup> Grops = new List<SignalRGroup>();

	

		// send messages
		public void SendToOperators(string message, string userName)
		{
			Clients.Group("Operators").showMessage(message, userName);
		}

		public void SendToDrivers(string message)
		{
			Clients.Group("Drivers").showMessage(message);
		}

		// Connect a new user
		public void Connect(int roleId)
		{
			var id = Context.ConnectionId;
			var currentUser = (new SignalRUser { ConnectionId = id, RoleId = roleId});

			if (!Users.Any(x => x.ConnectionId == id))
			{

				if (roleId == 1)
				{
					currentUser.Group = "Drivers";
					Groups.Add(Context.ConnectionId, "Drivers");

				}
				if (roleId == 2)
				{
					currentUser.Group = "Operators";
					Groups.Add(Context.ConnectionId, "Operators");
				}

				Users.Add(currentUser);

			}
		}

		// Disconnect user
		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				Users.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}