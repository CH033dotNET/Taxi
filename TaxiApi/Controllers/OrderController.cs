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
		readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
			OrderExDTO result;
			try
			{
				result =  orderManager.AddOrder(newOrder);
				LOGGER.Info("Order with id :{ " + result.Id + " } was created");
				return Json(result);
			}
			catch
			{
				LOGGER.Error("An error ocurred while adding order");
			}
			return Json("error");
		}

		[HttpGet]
		public IHttpActionResult GetOrderStatus(int order_id) {
			try {
				var result = orderManager.GetById(order_id);

				LOGGER.Info("Order with id: { " + result.Id + " } was requested, current status:"+result.Status);

				return Json(result.Status);

			} catch {

				LOGGER.Error("An error ocurred while requesting order with id: { "+order_id+" }");

				return Json("No order with this id");
			}
			

			
		}

	}
}
