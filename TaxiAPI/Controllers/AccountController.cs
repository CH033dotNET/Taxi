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
    }
}
