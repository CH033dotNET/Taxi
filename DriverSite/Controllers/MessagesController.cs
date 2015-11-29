using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using DriverSite.Helpers;

namespace DriverSite.Controllers
{
	public class MessagesController : ApiController
	{
		[HttpPost]
		public HttpResponseMessage OrderForDrivers(OrderDTO data)
		{
			MessagesHelper.OrderForDrivers(data);
			return Request.CreateResponse(HttpStatusCode.OK, data);
		}

		[HttpGet]
		public HttpResponseMessage ConfirmRequest(int data)
		{
			MessagesHelper.ConfirmRequest(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage RemoveAwaitOrder(int Id)
		{
			MessagesHelper.RemoveAwaitOrder(Id);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage DeniedRequest(int data)
		{
			MessagesHelper.DeniedRequest(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}

		[HttpGet]
		public HttpResponseMessage SendToDrivers(string data)
		{
			MessagesHelper.SendToDrivers(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
		public HttpResponseMessage hool()
		{
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
	}
}
