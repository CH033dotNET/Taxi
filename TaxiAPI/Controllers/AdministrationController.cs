using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using MainSaite.Models;
using Model.DTO;

namespace TaxiAPI.Controllers
{
    public class AdministrationController : ApiController
    {
		private IUserManager userManager;
		private IDriverManager driverManager;

		public AdministrationController(IUserManager userManager, IDriverManager driverManager)
		{
			this.userManager = userManager;
			this.driverManager = driverManager;
		}
    }
}
