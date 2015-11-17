using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using Model.DTO;

namespace TaxiAPI.Controllers
{
    public class DriveClientHistoryController : ApiController
    {
		private IOrderManager orderManager;
		public DriveClientHistoryController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}

		[HttpGet]
		[Route("api/DriveClientHistory/GetOrders")]
		public HttpResponseMessage GetOrders(int data)
		{
			var orders = orderManager.GetOrders();
			if (orders.Count() == 0)
			{
				Request.CreateResponse(HttpStatusCode.NoContent, orders);
			}

			return Request.CreateResponse(HttpStatusCode.OK, orders);
		}
    }
}
