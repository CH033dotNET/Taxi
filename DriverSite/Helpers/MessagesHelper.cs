using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DTO;
using DriverSite.Hubs;

namespace DriverSite.Helpers
{
	public class MessagesHelper
	{

		private static IHubContext Contest = GlobalHost.ConnectionManager.GetHubContext<DriverHub>();

		public static void OrderForDrivers(OrderDTO order)
		{
			Contest.Clients.All.newDriverOrders(order);
		}

		public static void ConfirmRequest(int data)
		{
			Contest.Clients.All.confirmDrRequest(data);  ////
			Contest.Clients.All.RemoveAwaitOrder(data);
		}

		public static void RemoveAwaitOrder(int Id)
		{
			Contest.Clients.All.removeAwaitOrders(Id);
		}

		public static void DeniedRequest(int data)
		{
			Contest.Clients.All.deniedDrRequest(data); ////
		}

		public static void SendToDrivers(string data)
		{
			Contest.Clients.All.showMessage(data);
		}
	}
}