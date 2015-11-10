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
		public HttpResponseMessage GetAddressesForUser(int data)
		{
			var address = addressmanager.GetAddressesForUser(data);
			if (address == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, address);
			}

			return Request.CreateResponse(HttpStatusCode.OK, address);
		}

		[HttpGet]
		[Route("api/UserPage/GetPersonByUserId")]
		public HttpResponseMessage GetPersonByUserId(int data)
		{
			var person = personManager.GetPersonByUserId(data);
			if (person == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, person);
			}

			return Request.CreateResponse(HttpStatusCode.OK, person);
		}

		[HttpGet]
		[Route("api/UserPage/GetOrdersByUserId")]
		public HttpResponseMessage GetOrdersByUserId(int data)
		{
			var order = orderManager.GetOrdersByUserId(data);
			if (order == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, order);
			}

			return Request.CreateResponse(HttpStatusCode.OK, order);
		}

		[HttpPost]
		[Route("api/UserPage/UpdatePhoneFMLnames")]
		public HttpResponseMessage UpdatePhoneFMLnames(PersonDTO data)
		{
			try
			{
				UpdatePhoneFMLnames(data);
				return Request.CreateResponse(HttpStatusCode.OK, 0);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, 1);
			}

		}
	}
}
