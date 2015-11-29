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
		private ILocationManager locationManager;


		public OrderController(IOrderManager orderManager, IPersonManager personManager, ICoordinatesManager coordinatesManager, ILocationManager locationManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
			this.coordinatesManager = coordinatesManager;
			this.locationManager = locationManager;
		}

		public ActionResult Index()
		{

			return View();
		}

		public JsonResult NewOrder(OrderDTO order)
		{
			order.PersonId = SessionPerson.Id;
			OrderDTO insertedOrder = orderManager.InsertOrder(order);

			insertedOrder.FirstName = personManager.GetPersons().FirstOrDefault(x => x.Id == insertedOrder.PersonId).FirstName;

			return Json(insertedOrder, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SetOrderStatus(int orderId, int status)
		{
			OrderDTO order = orderManager.GetOrderByOrderID(orderId);
			if (order != null)
			{
				order.IsConfirm = status;
				orderManager.EditOrder(order);
			}

			order.FirstName = personManager.GetPersons().FirstOrDefault(x => x.Id == order.PersonId).FirstName;
			//operatorHub.server.orderForDrivers(data); //////////////////!!!!!!!!!!!!!!!!!!!!!!!!!
			if (status == 1) ApiRequestHelper.postData<OrderDTO>("Orders", "OrderForDrivers", order);
			return Json(order, JsonRequestBehavior.AllowGet);
		}

		public JsonResult OrdersData()
		{
			var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 3).ToList();
			var peoples = personManager.GetPersons().ToList();

			var operatorOrders = from O in orders
								 join P in peoples
								 on O.PersonId equals P.Id
								 select new { OrderId = O.Id, FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };

			return Json(operatorOrders, JsonRequestBehavior.AllowGet);
		}

		public JsonResult DriversRequest()
		{
			/*var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId != 0).ToList();
			var peoples = personManager.GetPersons().ToList();
			var driverRequest = from O in orders
								join P in peoples
								on O.PersonId equals P.Id
								select new { OrderId = O.Id, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace, WaitingTime = O.WaitingTime, DriverId = O.DriverId };*/
			return Json(orderManager.GetDriverRequests().ToList(), JsonRequestBehavior.AllowGet);
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


		/// <summary>
		/// Action method that is executed when operator chooses a suitable driver for an order.
		/// </summary>
		/// <param name="orderId">id of an order</param>
		/// <param name="waitingTime">Time neede for taxi to reach client</param>
		/// <param name="driverId">id of a driver assigned to an order</param>
		/// <returns></returns>
		public JsonResult SetOrderToDriver(int orderId, string waitingTime, int driverId)
		{

			var order = orderManager.GetOrderByOrderID(orderId);
			order.WaitingTime = waitingTime;
			order.DriverId = driverId;

			var driverLocation = locationManager.GetByUserId(driverId); // find location by driver id

			order.DistrictId = driverLocation.DistrictId;
			order.District = driverLocation.District;
			var updatedOrder = orderManager.EditOrder(order);

			var driverCoordinates = coordinatesManager.GetCoordinatesByUserId(driverId).
					OrderByDescending(x => x.AddedTime).FirstOrDefault();

			ClientOrderedDTO currentOrder = new ClientOrderedDTO() { Latitude = driverCoordinates.Latitude, Longitude = driverCoordinates.Longitude, WaitingTime = waitingTime };

			ApiRequestHelper.Get<bool, int>("Orders", "ConfirmRequest", driverId); //////////////////!!!!!!!!!!!!!!!!!!!!!!!!!

			return Json(currentOrder, JsonRequestBehavior.AllowGet);

		}

		public JsonResult RemoveAwaitOrder(int orderId)
		{
			ApiRequestHelper.GetById<bool>("Orders", "RemoveAwaitOrder", orderId); //////////////////!!!!!!!!!!!!!!!!!!!!!!!!!
			return Json(true, JsonRequestBehavior.AllowGet);
		}

		public void DeniedRequest(int DriverId)
		{
			ApiRequestHelper.Get<bool, int>("Orders", "DeniedRequest", DriverId); //////////////////!!!!!!!!!!!!!!!!!!!!!!!!!
		}

		public JsonResult SendToDrivers(string message)
		{
			ApiRequestHelper.Get<bool, string>("Orders", "SendToDrivers", message); //////////////////!!!!!!!!!!!!!!!!!!!!!!!!!
			return Json(true, JsonRequestBehavior.AllowGet);
		}

	}
}

