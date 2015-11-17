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
    [HubName("OperatorHub")]
    public class OperatorHub: Hub
    {
        static ICollection<SignalRUser> operatorHubUsers = new List<SignalRUser>();

		[HubMethodName("sendToDrivers")]
		public void SendToDrivers(string message)
		{
			Clients.Group("Driver").showMessage(message);
		}


        [HubMethodName("orderForDrivers")]
        public void OrderForDrivers(OrderDTO order)
        {
            Clients.Group("Driver").newDriverOrders(order);
        }
        [HubMethodName("waitingOrderOp")]
        public void WaitingOrderOp(OrderDTO order)
        {
            Clients.Group("Operator").addWaitingOrder(order);
        }
        [HubMethodName("removeNewOrder")]
        public void RemoveNewOrder(OrderDTO order)
        {
            //Clients.All.removeNewOrders(order);
        }
        [HubMethodName("confirmRequest")]
        public void ConfirmRequest(int driverID)
        {
			var driverConnectionId = operatorHubUsers.FirstOrDefault(x => x.UserId == driverID).ConnectionId;
			Clients.Client(driverConnectionId).confirmDrRequest();
        }

        [HubMethodName("connectUser")]
        public void ConnectUser(int roleId, int userId)
        {
            string connectionId = Context.ConnectionId;
            int RoleId = roleId;

            var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = roleId, UserId = userId };
            if (!operatorHubUsers.Any(x => x.ConnectionId == connectionId))
            {
                if (roleId == 1)
                {
                    currentUser.Group = "Driver";
                    Groups.Add(Context.ConnectionId, "Driver");
                }
                if(roleId == 2)
                {
                    currentUser.Group = "Operator";
                    Groups.Add(Context.ConnectionId, "Operator");
                }
                operatorHubUsers.Add(currentUser);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            return base.OnDisconnected(stopCalled);
        }
    }
}