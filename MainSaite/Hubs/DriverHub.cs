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
    [HubName("DriverHub")]
    public class DriverHub:Hub
    {
        static ICollection<SignalRUser> drivers = new List<SignalRUser>();

        [HubMethodName("assignedOrder")]
        public void AssignedOrder(OrderDTO order)
        {
            Clients.Group("Operator").assignedDrOrder(order);
        }
        [HubMethodName("connectUser")]
        public void ConnectUser(int RoleId)
        {
            string connectionId = Context.ConnectionId;
            int roleId = RoleId;
            var currentUser = new SignalRUser() { ConnectionId = connectionId, RoleId = RoleId };
            if (!drivers.Any(x => x.ConnectionId == connectionId))
            {
                if (RoleId == 1)
                {
                    currentUser.Group = "Driver";
                    Groups.Add(Context.ConnectionId, "Driver");
                }
                if (RoleId == 2)
                {
                    currentUser.Group = "Operator";
                    Groups.Add(Context.ConnectionId, "Operator");
                }
                drivers.Add(currentUser);
            }
        }
        [HubMethodName("privateDriverLine")]
        public void PrivateDriverLine(int DriverID)
        {
            string connectionId = Context.ConnectionId;
            int driverID = DriverID;
            var currentDriver = new SignalRUser() { ConnectionId = connectionId, RoleId = DriverID };
            if (!drivers.Any(x => x.ConnectionId == connectionId))
            {
                currentDriver.Group = driverID.ToString();
                Groups.Add(Context.ConnectionId, driverID.ToString());
            }
            drivers.Add(currentDriver);
        }


    }
}