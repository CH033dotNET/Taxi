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
	[HubName("OrderHub")]
	public class OrderHub : Hub
	{
		static ICollection<SignalRUserEx> orderHubUsers = new List<SignalRUserEx>();

		[HubMethodName("addOrder")]
		public void AddOrder(OrderExDTO order)
		{
			Clients.Group("Operator").addOrder(order);
		}

        [HubMethodName("OrderApproved")]
        public void OrderApproved(OrderExDTO order) {
            Clients.Group("Driver").OrderApproved(order);
        }

        [HubMethodName("OrderConfirmed")]
        public void OrderConfirmed(OrderExDTO order) {
           // var user = orderHubUsers.FirstOrDefault(x=> x. == );
          //  Clients.
        }


        [HubMethodName("connect")]
		public void Connect(string group)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUserEx() { ConnectionId = connectionId, Group = group};
			if (!orderHubUsers.Any(x => x.ConnectionId == connectionId))
			{
				Groups.Add(Context.ConnectionId, group);
				orderHubUsers.Add(currentUser);
			}
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var item = orderHubUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (item != null)
			{
				orderHubUsers.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}