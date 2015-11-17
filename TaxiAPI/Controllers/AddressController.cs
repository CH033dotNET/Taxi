using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;
using Model.DTO;
using Common.Helpers;
using Common.Enum;

namespace TaxiAPI.Controllers
{
    public class AddressController : ApiController
    {
		private IAddressManager addressmanager;
		public AddressController(IAddressManager addressmanager)
		{
			this.addressmanager = addressmanager;
		}

		[HttpGet]
		[Route("api/Address/GetAddressesForUser")]
		public HttpResponseMessage GetAddressesForUser(int data)
		{
			var addresses = addressmanager.GetAddressesForUser(data);
			if (addresses.Count() == 0)
			{
				return Request.CreateResponse(HttpStatusCode.NoContent, addresses);
			}

			return Request.CreateResponse(HttpStatusCode.OK, addresses);
		}

		[HttpGet]
		[Route("api/Address/GetById")]
		public HttpResponseMessage GetById(int data)
		{
			var addresses = addressmanager.GetById(data);
			if (addresses.AddressId == 0)
			{
				return Request.CreateResponse(HttpStatusCode.NoContent, addresses);
			}
			return Request.CreateResponse(HttpStatusCode.OK, addresses);
		}

		[HttpGet]
		[Route("api/Address/DeleteAddress")]
		public HttpResponseMessage DeleteAddress(int data)
		{
			try
			{
				addressmanager.DeleteAddress(data);
				return Request.CreateResponse(HttpStatusCode.OK, 0);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, 1);
			}
		}

		[HttpPost]
		[Route("api/Address/UpdateAddress")]
		public HttpResponseMessage UpdateAddress(AddressDTO data)
		{
			var myAddress = addressmanager.UpdateAddress(data);
			if (myAddress == null)
			{
				return Request.CreateResponse(HttpStatusCode.NoContent, myAddress);
			}
			return Request.CreateResponse(HttpStatusCode.OK, myAddress);
		}

		[HttpPost]
		[Route("api/Address/AddAddress")]
		public HttpResponseMessage AddAddress(AddressDTO data)
		{
			var myAddress = addressmanager.AddAddress(data);
			return Request.CreateResponse(HttpStatusCode.OK, myAddress);
		}

    }
}
