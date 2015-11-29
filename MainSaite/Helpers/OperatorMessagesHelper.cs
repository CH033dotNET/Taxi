using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MainSaite.Hubs;
using Model.DTO;

namespace MainSaite.Helpers
{
	public class OperatorMessagesHelper
	{
		private static IHubContext Contest = GlobalHost.ConnectionManager.GetHubContext<DriversLocationHub>();
		private static IHubContext driverContext = GlobalHost.ConnectionManager.GetHubContext<DriverHub>();


		public static void SendToOperators(string message, string userName)
		{
			driverContext.Clients.All.showMessageToOperators(message, userName);
		}

		public static void AssignedOrder(object order)
		{
			driverContext.Clients.Group("Operator").assignedDrOrder(order);
		}
	}
}
