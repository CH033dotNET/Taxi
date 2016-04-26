using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using Model.DB;

namespace TaxiAPI.Controllers
{
	public class OrderController : ApiController
	{
		private IOrderManager orderManager;
		private IPersonManager personManager;

		public OrderController(IOrderManager orderManager, IPersonManager personManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
		}

		[HttpPost]
		[Route("api/Order/NewOrder")]
		public HttpResponseMessage NewOrder(OrderDTO order)
		{
			order = orderManager.InsertOrder(order);

			order.FirstName = personManager.GetPersons().FirstOrDefault(x => x.Id == order.PersonId).FirstName;

			return Request.CreateResponse(HttpStatusCode.OK, order);
		}


	}
}
