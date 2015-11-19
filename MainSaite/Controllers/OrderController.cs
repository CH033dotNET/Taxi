﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DB;
using BAL.Manager;
using Model.DTO;
using Model;

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

		public JsonResult NewOrder(OrderDTO order)
		{
			order.PersonId = SessionPerson.Id;
			//order.DriverId = 0;
			//order.District = new District() { Name = "unknown", Id = 0 };
			var insertedOrder = orderManager.InsertOrder(order);
	
			return Json(insertedOrder,JsonRequestBehavior.AllowGet);
		}

		public JsonResult SetOrderStatus(int orderId, int status)
		{
			var order = orderManager.GetOrderByOrderID(orderId);
			if (order != null)
			{
				order.IsConfirm = status;
				orderManager.EditOrder(order);
			}
            return Json(order, JsonRequestBehavior.AllowGet);
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
				{IsConfirm = order.IsConfirm, Latitude = driverCoordinates.Latitude, Longitude = driverCoordinates.Longitude, WaitingTime = order.WaitingTime};

				return Json(currentOrder, JsonRequestBehavior.AllowGet);
			}

			else
			{
				return Json(order, JsonRequestBehavior.AllowGet);
			}
		}

		//public JsonResult CloseOrder()
		//{ 
		
		
		//}
    }
}

