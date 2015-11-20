using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using MainSaite.Models;
using Model.DTO;


namespace MainSaite.Hubs
{

	[HubName("ClientHub")]
	public class ClientHub : Hub
	{
		static ICollection<SignalRUser> clientHubUsers = new List<SignalRUser>();

		[HubMethodName("connectUser")]
		public void ConnectUser(int roleId, int userId)
		{
			string connectionId = Context.ConnectionId;

			var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = roleId, UserId = userId };
			if (!clientHubUsers.Any(x => x.ConnectionId == connectionId))
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

				if (roleId == 3)
				{
					currentUser.Group = "Client";
					Groups.Add(Context.ConnectionId, "Client");
				}


				clientHubUsers.Add(currentUser);
			}
		}


		[HubMethodName("sendNewOrderToOperators")]
		public void SendNewOrderToOperators(OrderDTO newOrder)
		{
			Clients.Group("Operator").newOrderFromClient(newOrder);
		}


	}
}