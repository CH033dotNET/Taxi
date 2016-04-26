using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientSite.Models;
using Model.DTO;
namespace ClientSite.Hubs
{
	[HubName("OperatorHub")]
	public class OperatorHub : Hub
	{
		static ICollection<SignalRUser> clients = new List<SignalRUser>();

		private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<OperatorHub>();
		
		//deny Client Order
		//[HubMethodName("deniedClientOrder")]

		public static void DeniedClientOrder(int clientUserId)
		{
			var clientConnectionId = clients.FirstOrDefault(x => x.UserId == clientUserId).ConnectionId;
			hubContext.Clients.Client(clientConnectionId).deniedClientOrder();
		}


		//Show modal form 'no free cars'
		//[HubMethodName("noFreeCarClientOrder")]
		public static void NoFreeCarClientOrder(int clientId)
		{
			var clientConnectionId = clients.FirstOrDefault(x => x.UserId == clientId).ConnectionId;
			hubContext.Clients.Client(clientConnectionId).noFreeCar();
		}


		//Show modal form 'wait taxi' to client
		//[HubMethodName("confirmClientOrder")]
		public static void ConfirmClientOrder(ClientOrderedDTO order)
		{
			var clientConnectionId = clients.FirstOrDefault(x => x.UserId == order.userId).ConnectionId;
			hubContext.Clients.Client(clientConnectionId).waitYourCar(order.WaitingTime, order.Latitude, order.Longitude);
		}

		[HubMethodName("connectUser")]
		public void ConnectUser(int roleId, int userId)
		{
			string connectionId = Context.ConnectionId;

			int RoleId = roleId;

			var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = roleId, UserId = userId };

			if (!clients.Any(x => x.ConnectionId == connectionId))
			{
				if (roleId == 3)
				{
					currentUser.Group = "Client";
					Groups.Add(Context.ConnectionId, "Client");
				}
				clients.Add(currentUser);

			}
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				clients.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}