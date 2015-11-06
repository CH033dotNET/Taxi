using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MainSaite.Models;
using Model.DTO;

namespace TaxiAPI.Controllers
{
	public class UserPageController : ApiController
	{
		private IAddressManager addressmanager;
		private IPersonManager personManager;
		private IOrderManager orderManager;
		public UserPageController(IAddressManager addressmanager, IPersonManager personManager, IOrderManager orderManager)
		{
			this.addressmanager = addressmanager;
			this.personManager = personManager;
			this.orderManager = orderManager;
		}

		[HttpGet]
		[Route("api/UserPage/GetAddressesForUser")]
		public HttpResponseMessage GetAddressesForUser(int id)
		{
			var address = addressmanager.GetAddressesForUser(id);
			if (address == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, address);
			}

			return Request.CreateResponse(HttpStatusCode.OK, address);
		}

		[HttpGet]
		[Route("api/UserPage/GetPersonByUserId")]
		public HttpResponseMessage GetPersonByUserId(int id)
		{
			var person =  personManager.GetPersonByUserId(id);
			if (person == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, person);
			}

			return Request.CreateResponse(HttpStatusCode.OK, person);
		}

		[HttpGet]
		[Route("api/UserPage/GetOrdersByUserId")]
		public HttpResponseMessage GetOrdersByUserId(int id)
		{
			var order = orderManager.GetOrdersByUserId(id);
			if (order == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, order);
			}

			return Request.CreateResponse(HttpStatusCode.OK, order);
		}

		[HttpPost]
		[Route("api/UserPage/UpdatePhoneFMLnames")]
		public HttpResponseMessage UpdatePhoneFMLnames(PersonDTO person)
		{
			try
			{
				UpdatePhoneFMLnames(person);
				return Request.CreateResponse(HttpStatusCode.OK, 0);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, 1);
			}
			
		}
	}
}
