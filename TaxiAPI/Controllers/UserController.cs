using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;

namespace TaxiAPI.Controllers
{
	public class UserController : ApiController
	{
		private IPersonManager personManager;

		public UserController(IPersonManager personManager)
		{
			this.personManager = personManager;
		}

		[HttpGet]
		[Route("api/User/GetPersonByUserId")]
		public HttpResponseMessage GetPersonByUserId(int? id)
		{
			var person = personManager.GetPersonByUserId(id);
			if (person == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, person);
			}

			return Request.CreateResponse(HttpStatusCode.OK, person);
		}

		[HttpPost]
		[Route("api/User/EditPerson")]
		public HttpResponseMessage EditPerson(PersonDTO person)
		{
			var pers = personManager.EditPerson(person);
			if (pers == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound, pers);
			}

			return Request.CreateResponse(HttpStatusCode.OK, pers);
		}
	}
}
