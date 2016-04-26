using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using ClientSite.Helpers;

namespace ClientSite.Controllers
{
	public class MessagesController : ApiController
	{
		[HttpGet]
		[Route("api/Messages/DenyOrder")]
		public HttpResponseMessage DenyOrder(int data)
		{
			OrderHelper.DeniedRequest(data);

			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
		[HttpGet]
		public HttpResponseMessage NoFreeCar(int data)
		{
			OrderHelper.NoFreeCar(data);

			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
		[HttpPost]
		public HttpResponseMessage WaitYourCar(ClientOrderedDTO data)
		{
			//OrderDTO o = order as OrderDTO;
			//var i = 0;
			OrderHelper.WaitYourCar(data);

			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
	}
}