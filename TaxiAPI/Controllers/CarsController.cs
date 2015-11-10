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
using BAL.Interfaces;
//using FluentValidation?

namespace TaxiAPI.Controllers
{
    public class CarsController : BaseController
    {
		private ICarManager carManager;
		private IUserManager userManager;
		private IWorkerStatusManager wsManager;
		public CarsController(ICarManager carManager, IUserManager userManager, IWorkerStatusManager wsManager)
		{
			this.carManager = carManager;
			this.userManager = userManager;
			this.wsManager = wsManager;
		}

		[HttpGet]
		[Route("api/Cars/GetCarsBy")]
		public HttpResponseMessage GetCarsBy(int id)
		{
			var cars = carManager.getCarsByUserID(id).ToList();
			return Request.CreateResponse(HttpStatusCode.OK, cars);
		}

		[HttpGet]
		public HttpResponseMessage GetCarsBy(int id, string paramater)
		{
			var cars = carManager.getCarsByUserID(id,paramater).ToList();
			return Request.CreateResponse(HttpStatusCode.OK, cars);
		}

		[HttpGet]
		[Route("api/Cars/GetDriversBy")]
		public HttpResponseMessage GetDriversBy(int id)
		{
			var drivers = userManager.GetDriversExceptCurrent(id);
			return Request.CreateResponse(HttpStatusCode.OK, drivers);
		}

		[HttpPost]
		public HttpResponseMessage PostCar(CarDTO car)
		{
			carManager.addCar(car);
			return Request.CreateResponse(HttpStatusCode.Created);
		}

		[HttpDelete]
		public HttpResponseMessage DeleteCarBy(int id)
		{
			var message = carManager.deleteCarByID(id);
			return Request.CreateResponse(HttpStatusCode.OK, message);
		}

		[HttpGet]
		public HttpResponseMessage GetCarBy(int Id)
		{
			var carForEdit = carManager.GetCarByCarID(Id);
			return Request.CreateResponse(HttpStatusCode.OK, carForEdit);
		}

		[HttpPut]
		public HttpResponseMessage PutCar(CarDTO modelObj)
		{
			var newCar = carManager.EditCar(modelObj);
			return Request.CreateResponse(HttpStatusCode.Created, newCar);
		}

		[HttpPut]
		public HttpResponseMessage GiveAwayCar(int id, int Id2)
		{
			var statusMessage = carManager.GiveAwayCar(id, Id2);
			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPut]
		public HttpResponseMessage SetCarStatus(int id, int id2)
		{
			var statusMessage = carManager.ChangeCarToMain(id, id2);
			return Request.CreateResponse(HttpStatusCode.Created);
		}

		//[HttpGet]
		//public HttpResponseMessage GetCar()
		//{

		//	//var newcar = this.carManager.getCars();
		//	var newcar = this.carManager.GetCarByCarID(9);
		//	//TestManager testman = new TestManager();
		//	//var car = testman.GetCar();
		//	//return car;
		//	return Request.CreateResponse(HttpStatusCode.OK, newcar);
		//}

		//тот же код
		//[HttpGet]
		//public HttpResponseMessage GetCar2()
		//{
		//	var newcar = this.carManager.GetCarByCarID(9);
		//	if (newcar == null)
		//	{
		//		return Request.CreateResponse(HttpStatusCode.NoContent, "no items were found");
		//	}
		//	else
		//	{
		//		Request.CreateResponse(HttpStatusCode.OK, newcar);
		//	}
		//}
    }
}
