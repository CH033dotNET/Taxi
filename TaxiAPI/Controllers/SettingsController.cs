using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
//using MainSaite.Models;

namespace TaxiAPI.Controllers
{
	public class SettingsController : ApiController
	{
		private IUserManager userManager;
		private IDistrictManager districtManager;
		private ICarManager carManager;
		public SettingsController(IUserManager userManager, IDistrictManager districtManager, ICarManager carManager)
		{
			this.userManager = userManager;
			this.districtManager = districtManager;
			this.carManager = carManager;
		}
	}
}
