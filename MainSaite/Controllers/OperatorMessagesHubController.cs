using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using MainSaite.Helpers;

namespace MainSaite.Controllers
{
	public class OperatorMessagesHubController : ApiController
	{

		[HttpPost]
		public HttpResponseMessage assignedOrder(object data)
		{
			OperatorMessagesHelper.AssignedOrder(data);
			return Request.CreateResponse(HttpStatusCode.OK, data);
		}
		[HttpGet]
		public HttpResponseMessage SendToOperators(string param1, string param2)
		{
			OperatorMessagesHelper.SendToOperators(param1, param2);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
	}
}
