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

    }
}
