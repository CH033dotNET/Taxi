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
using Model.DB;


namespace TaxiAPI.Controllers
{
    public class AccountController : ApiController
    {
		private IUserManager userManager;
		private IPersonManager personManager;

		public AccountController(IUserManager userManager, IPersonManager personManager)
		{
			this.userManager = userManager;
			this.personManager = personManager;
		}

		[HttpPost]
		[Route("api/Account/getUser")]
		public HttpResponseMessage getUser(LoginModel data)
		{
			var my = userManager.GetByUserName(data.UserName, data.Password);
			if (my == null) return Request.CreateResponse(HttpStatusCode.NotFound, my);
			return Request.CreateResponse(HttpStatusCode.OK, my);
		}

		[HttpGet]
		[Route("api/Account/getPerson")]
		public HttpResponseMessage getPerson(int data)
		{
			var person = personManager.GetPersonByUserId(data);
			if (person == null)
			{
				person = personManager.InsertPerson(new PersonDTO() { UserId = data, ImageName = "item_0_profile.jpg" });
			}
			return Request.CreateResponse(HttpStatusCode.OK, person);
		}

		[HttpGet]
		[Route("api/Account/IfUserNameExists")]
		public HttpResponseMessage IfUserNameExists(string data)
		{
			var exist = userManager.IfUserNameExists(data);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/IfEmailExists")]
		public HttpResponseMessage IfEmailExists(string data)
		{
			var exist = userManager.IfEmailExists(data);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/IsUserNameCorrect")]
		public HttpResponseMessage IsUserNameCorrect(string data)
		{
			var exist = userManager.IsUserNameCorrect(data);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/DefaultImage")]
		public HttpResponseMessage DefaultImage(int data)
		{
			var defImg = personManager.DefaultImage(data);
			if (defImg == null) return Request.CreateResponse(HttpStatusCode.NoContent, defImg);
			return Request.CreateResponse(HttpStatusCode.OK, defImg);
		}

		[HttpPost]
		[Route("api/Account/InsertUser")]
		public HttpResponseMessage InsertUser(RegistrationModel data)
		{
			var IsInsert = userManager.InsertUser(data);
			return Request.CreateResponse(HttpStatusCode.OK, IsInsert);
		}

		[HttpPost]
		[Route("api/Account/InsertPerson")]
		public HttpResponseMessage InsertPerson(PersonDTO data)
		{
			var IsInsert = personManager.InsertPerson(data);
			return Request.CreateResponse(HttpStatusCode.OK, IsInsert);
		}

    }
}
