using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using MainSaite.Models;
using Model.DTO;

namespace TaxiAPI.Controllers
{
	public class OrdersController : BaseController
	{
		private IOrderManager orderManager;
		private ICoordinatesManager coordinatesManager;

		public OrdersController(IOrderManager orderManager, ICoordinatesManager coordinatesManager)
		{
			this.coordinatesManager = coordinatesManager;
			this.orderManager = orderManager;
		}
		[HttpGet]
		public HttpResponseMessage GetOrders()
		{
			var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0).ToList();
			return Request.CreateResponse(HttpStatusCode.OK, orders);
		}

        [HttpGet]
        public HttpResponseMessage GetTodayOrders() {
            var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0 && x.OrderTime.Date == DateTime.Now.Date).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, orders);
        }

        [HttpGet]
		public HttpResponseMessage GetCurrentOrder(int Id)
		{
			var currentOrder = orderManager.GetOrderByOrderID(Id);
			//MainSiteRequestHelper.postData<OrderDTO>("OperatorMessagesHub", "assignedOrder", currentOrder);
			return Request.CreateResponse(HttpStatusCode.OK, currentOrder);
		}

		[HttpGet]
		public HttpResponseMessage SendToOperators(string param1, string param2)
		{

			var sended = MainSiteRequestHelper.Get<bool, string, string>("OperatorMessagesHub", "SendToOperators", param1, param2).Data;
			//var currentOrder = orderManager.GetOrderByOrderID(orderId);
			return Request.CreateResponse(HttpStatusCode.OK, sended);
		}
		//-------------------------------------------------------------------------------------------------
		[HttpPost]
		public HttpResponseMessage OrderForDrivers(OrderDTO data)
		{
			DriverRequestHelper.postData<OrderDTO>("Messages", "OrderForDrivers", data);
			return Request.CreateResponse(HttpStatusCode.OK, data);
		}

		[HttpGet]
		public HttpResponseMessage ConfirmRequest(int data)
		{
			DriverRequestHelper.Get<bool, int>("Messages", "ConfirmRequest", data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage RemoveAwaitOrder(int Id)
		{
			DriverRequestHelper.GetById<bool>("Messages", "RemoveAwaitOrder", Id);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage DeniedRequest(int data)
		{
			DriverRequestHelper.Get<bool, int>("Messages", "DeniedRequest", data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage SendToDrivers(string data)
		{
			DriverRequestHelper.Get<bool, string>("Messages", "SendToDrivers", data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpPost]
		public HttpResponseMessage AssignCurrentOrder(OrderDTO data)
		{
			MainSiteRequestHelper.postData<OrderDTO>("OperatorMessagesHub", "assignedOrder", data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpPost]
		public HttpResponseMessage SetCoordinates(CoordinatesDTO data)
		{
			coordinatesManager.AddCoordinates(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
	}
}
