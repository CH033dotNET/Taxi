using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.DTO;
using ClientSite.Hubs;

namespace ClientSite.Helpers
{
	public class OrderHelper
	{
		public static void DeniedRequest(int userId)
		{
			OperatorHub.DeniedClientOrder(userId);
		}
		public static void NoFreeCar(int userId)
		{
			OperatorHub.NoFreeCarClientOrder(userId);
		}
		public static void WaitYourCar(ClientOrderedDTO order)
		{
			OperatorHub.ConfirmClientOrder(order);
		}
		//public static void DeniedOrder()
		//{
		//	Context.Clients.All.
		//}


	}
}