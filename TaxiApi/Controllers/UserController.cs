using BAL.Manager;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxiApi.Controllers
{
	public class UserController : ApiController
	{
		private readonly IUserManager userManger;

		public UserController(IUserManager userManger)
		{
			this.userManger = userManger;
		}

		[HttpGet]
		public IHttpActionResult Login(string login, string password)
		{
			return Json(userManger.GetByUserName(login, password));
		}


	}
}
