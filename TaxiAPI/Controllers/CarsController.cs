using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Tools;
using Common.Enum;
using MainSaite.Models;
using Model.DTO;
using BAL.Manager;
//using FluentValidation?

namespace TaxiAPI.Controllers
{
    public class CarsController : ApiController
    {
		private ICarManager carManager;
		private IUserManager userManager;
		public CarsController(ICarManager carManager, IUserManager userManager)
		{
			this.carManager = carManager;
			this.userManager = userManager;
		}
    }
}
