using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Interfaces;
using BAL.Manager;
using Model.DTO;

namespace TaxiApi.Controllers
{
    public class OrderController : ApiController
    {

		private readonly IOrderManagerEx orderManager;

		public OrderController(IOrderManagerEx orderManager) {
			this.orderManager = orderManager;
		}

		[HttpGet]
		//public IHttpActionResult CreateOrder(OrderExDTO newOrder) {
		public IHttpActionResult CreateOrder(string address) {
			var newOrder = new OrderExDTO();
			var adrs = new AddressFromDTO();
			adrs.Address = address;
			newOrder.AddressFrom = adrs;

			var result = orderManager.AddOrder(newOrder);
			return Json(result);

		}

		[HttpGet]
		public IHttpActionResult GetOrderStatus(int order_id) {
			try {
				var result = orderManager.GetById(order_id);
				return Json(result.Status);
			} catch {
				return Json("No order with this id");
			}
			

			
		}

	}
}
