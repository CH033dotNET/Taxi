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
		public HttpResponseMessage GetAddressesForUser(int id)
		{
			var addresses = addressmanager.GetAddressesForUser(id);
			if (addresses == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, addresses);
			}

			return Request.CreateResponse(HttpStatusCode.OK, addresses);
		}

		[HttpGet]
		[Route("api/Address/GetById")]
		public HttpResponseMessage GetById(int id)
		{
			var addresses = addressmanager.GetById(id);
			if (addresses == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, addresses);
			}
			return Request.CreateResponse(HttpStatusCode.OK, addresses);
		}

		[HttpGet]
		[Route("api/Address/DeleteAddress")]
		public HttpResponseMessage DeleteAddress(int id)
		{
			try
			{
				addressmanager.DeleteAddress(id);
				return Request.CreateResponse(HttpStatusCode.OK, 0);
			}
			catch (Exception)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, 1);
			} 
		}

		[HttpPost]
		[Route("api/Address/UpdateAddress")]
		public HttpResponseMessage UpdateAddress(AddressDTO address)
		{
			var myAddress = addressmanager.UpdateAddress(address);
			if (myAddress == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, myAddress);
			}
			return Request.CreateResponse(HttpStatusCode.OK, myAddress);
		}

		[HttpPost]
		[Route("api/Address/GetAddressesForUser")]
		public HttpResponseMessage AddAddress(AddressDTO address)
		{
			var myAddress = addressmanager.AddAddress(address);
			if (myAddress == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, myAddress);
			}
			return Request.CreateResponse(HttpStatusCode.OK, myAddress);
		}

    }
}
