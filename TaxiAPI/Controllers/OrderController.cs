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

		public OrderController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}
	}
}
