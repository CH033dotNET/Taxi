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
		readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IUserManager userManger;

		public UserController(IUserManager userManger)
		{
			this.userManger = userManger;
		}

		[HttpGet]
		public IHttpActionResult Login(string login, string password)
		{
			var user = userManger.GetByUserName(login, password);
			if(user != null)
			   LOGGER.Info("User with Login: {"+login+"} loged in");
			else if (user == null)
				LOGGER.Error("User with login {"+login+"} tried to enter, but there is no such user");

			return Json(user);
		}


	}
}
