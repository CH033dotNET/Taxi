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
        public HttpResponseMessage getPerson(int id)
        {
            var person = personManager.GetPersonByUserId(id);
            if (person == null)
            {
                person = personManager.InsertPerson(new PersonDTO() { UserId = id, ImageName = "item_0_profile.jpg" });
            }
           /* if (!System.IO.File.Exists(Server.MapPath(@"~\Images\") + currentPerson.ImageName))
            {
                personManager.DefaultImage(user.Id);
            }*/
            return Request.CreateResponse(HttpStatusCode.OK, person);
        }

		[HttpGet]
		[Route("api/Account/IfUserNameExists")]
		public HttpResponseMessage IfUserNameExists(string userName)
		{
			var exist = userManager.IfUserNameExists(userName);
			if (exist == null) return Request.CreateResponse(HttpStatusCode.NotFound, exist);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/IfEmailExists")]
		public HttpResponseMessage IfEmailExists(string userName)
		{
			var exist = userManager.IfEmailExists(userName);
			if (exist == null) return Request.CreateResponse(HttpStatusCode.NotFound, exist);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/IsUserNameCorrect")]
		public HttpResponseMessage IsUserNameCorrect(string userName)
		{
			var exist = userManager.IsUserNameCorrect(userName);
			if (exist == null) return Request.CreateResponse(HttpStatusCode.NotFound, exist);
			return Request.CreateResponse(HttpStatusCode.OK, exist);
		}

		[HttpGet]
		[Route("api/Account/DefaultImage")]
		public HttpResponseMessage DefaultImage(int UserId)
		{
			var defImg = personManager.DefaultImage(UserId);
			if (defImg == null) return Request.CreateResponse(HttpStatusCode.NotFound, defImg);
			return Request.CreateResponse(HttpStatusCode.OK, defImg);
		}

		[HttpPost]
		[Route("api/Account/InsertUser")]
		public HttpResponseMessage InsertUser(UserDTO user)
		{
			var IsInsert = userManager.InsertUser(user);
			if (IsInsert == null) return Request.CreateResponse(HttpStatusCode.NotFound, IsInsert);
			return Request.CreateResponse(HttpStatusCode.OK, IsInsert);
		}

		[HttpPost]
		[Route("api/Account/InsertPerson")]
		public HttpResponseMessage InsertPerson(PersonDTO person)
		{
			var IsInsert = personManager.InsertPerson(person);
			if (IsInsert == null) return Request.CreateResponse(HttpStatusCode.NotFound, IsInsert);
			return Request.CreateResponse(HttpStatusCode.OK, IsInsert);
		}

    }
}
