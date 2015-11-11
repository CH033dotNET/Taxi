using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DB;
using BAL.Manager;
using Model.DTO;

namespace MainSaite.Controllers
{
    public class OrderController : BaseController
    {
		private IOrderManager orderManager;
		private IPersonManager personManager;
		private ICoordinatesManager coordinatesManager;


		public OrderController(IOrderManager orderManager, IPersonManager personManager, ICoordinatesManager coordinatesManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
			this.coordinatesManager = coordinatesManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetOrder(OrderDTO order)
		{
			order.PersonId = SessionPerson.Id;
			var insertOrder = orderManager.InsertOrder(order);
			
			var orderId = insertOrder.Id;
			return Json(orderId,JsonRequestBehavior.AllowGet);
		}

		public void SetOrderStatus(int orderId, int status)
		{
			var order = orderManager.GetOrderByOrderID(orderId);
			if (order != null)
			{
				order.IsConfirm = status;
				orderManager.EditOrder(order);
			}
		}

		public JsonResult OrdersData()
		{
			var orders = orderManager.GetOrders().Where(x=>x.IsConfirm == 3).ToList();
			var peoples = personManager.GetPersons().ToList();

			var operatorOrders = from O in orders
								 join P in peoples
								 on O.PersonId equals P.Id
								 select new { OrderId = O.Id, FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };

			return Json(operatorOrders, JsonRequestBehavior.AllowGet);
		}

		public JsonResult DriversRequest()
		{ 
		    var orders =  orderManager.GetOrders().Where(x=>x.IsConfirm == 1 && x.DriverId != 0).ToList();
			var peoples = personManager.GetPersons().ToList();
			var driverRequest = from O in orders
								join P in peoples
								on O.PersonId equals P.Id
								select new { OrderId = O.Id, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace, WaitingTime = O.WaitingTime, DriverId = O.DriverId };
			return Json(driverRequest, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetWaitingOrders()
		{
			var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0).ToList();
			var peoples = personManager.GetPersons().ToList();

			var operatorOrders = from O in orders
								 join P in peoples
								 on O.PersonId equals P.Id
								 select new { OrderId = O.Id, FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };

			return Json(operatorOrders, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetOrderedTaxi(int orderId)
		{

			var order = orderManager.GetOrderByOrderID(orderId);
			if (order.IsConfirm == 4)
			{
				var driverCoordinates = coordinatesManager.GetCoordinatesByUserId(order.DriverId).
					OrderByDescending(x => x.AddedTime).FirstOrDefault();

				ClientOrderedDTO currentOrder = new ClientOrderedDTO() 
				{Latitude = driverCoordinates.Latitude, Longitude = driverCoordinates.Longitude, WaitingTime = order.WaitingTime};

				return Json(currentOrder, JsonRequestBehavior.AllowGet);
			}

			if (order.IsConfirm == 2)
			{
				return Json("denied", JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json("wait", JsonRequestBehavior.AllowGet);
			}
		}

		//public JsonResult CloseOrder()
		//{ 
		
		
		//}
    }
}

