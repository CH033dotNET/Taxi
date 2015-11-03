using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Tools;
using Common.Enum;
using Model.DTO;
using BAL.Manager;
//using FluentValidation?

namespace TaxiAPI.Controllers
{
    public class CarsController : BaseController
    {
		private ICarManager carManager;
		private IUserManager userManager;
		public CarsController(ICarManager carManager, IUserManager userManager)
		{
			this.carManager = carManager;
			this.userManager = userManager;
		}

		[HttpGet]
		public CarDTO GetCar()
		{

			//var newcar = this.carManager.getCars();
			var newcar = this.carManager.GetCarByCarID(9);
			//TestManager testman = new TestManager();
			//var car = testman.GetCar();
			//return car;
			return newcar;
		}
    }
}
