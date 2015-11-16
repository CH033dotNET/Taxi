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
        static ICollection<SignalRUser> operators = new List<SignalRUser>();
      //  static ICollection<SignalRGroup> groups = new List<SignalRGroup>();

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
            Clients.Group(driverID.ToString()).confirmDrRequest();
        }
        [HubMethodName("connectUser")]
        public void ConnectUser(int RoleId)
        {
            string connectionId = Context.ConnectionId;
            int roleId = RoleId;
            var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = RoleId };
            if (!operators.Any(x => x.ConnectionId == connectionId))
            {
                if (RoleId == 1)
                {
                    currentUser.Group = "Driver";
                    Groups.Add(Context.ConnectionId, "Driver");
                }
                if(RoleId == 2)
                {
                    currentUser.Group = "Operator";
                    Groups.Add(Context.ConnectionId, "Operator");
                }
                operators.Add(currentUser);
            }
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {

            return base.OnDisconnected(stopCalled);
        }
    }
}