#define WORKING
using BAL.Tools;
using Common.Enum;
using MainSaite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using BAL.Manager;
using RestSharp;
using BAL.Interfaces;

namespace MainSaite.Controllers
{
    public class CarsController : BaseController
	{
#region CarControllers
		#if WORKING
		private ICarManager carManager;
		private IUserManager userManager;
		private IWorkerStatusManager wsManager;
		public CarsController(ICarManager carManager, IUserManager userManager, IWorkerStatusManager wsManager)
		{
			this.carManager = carManager;
			this.userManager = userManager;
			this.wsManager = wsManager;
		}


		/// <summary>
		/// Action method, returns main view and populates it with list of objects
		/// </summary>
		/// <returns></returns>
		public ActionResult CarPark()
		{
			AutoParkViewModel CarsViewModel = new AutoParkViewModel();

			if (SessionUser == null || SessionUser.RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				int userId = SessionUser.Id;
				CarsViewModel.Cars = carManager.getCarsByUserID(userId).ToList();
				CarsViewModel.Drivers = userManager.GetDriversExceptCurrent(userId);
				return View(CarsViewModel);
			}
		}

		public JsonResult SortBy(string parameter)
		{
			IEnumerable<CarDTO> DriversCars;
			int userId = SessionUser.Id;
			DriversCars = carManager.getCarsByUserID(userId, parameter).ToList();
			return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Gets an object from ajax call and adds it to database
		/// </summary>
		/// <param name="car">object from ajax call</param>
		/// <returns></returns>
		public JsonResult AddNewCar(CarDTO car)
		{
			IEnumerable<CarDTO> DriversCars;
			int userId = SessionUser.Id;
			CarModelValidator validator = new CarModelValidator();
			var checkedCar = validator.Validate(car, ruleSet: "AddNewCar");
			if (!checkedCar.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				carManager.addCar(car);
				DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Get an id from ajax call and deletes a car object with current id
		/// </summary>
		/// <param name="Id">Id from ajax call</param>
		/// <returns></returns>
		public JsonResult DeleteCar(int Id)
		{
			var statusMessage = carManager.deleteCarByID(Id);
			if (statusMessage == "Failure")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Get an id from ajax call and returns a car object with current id.
		/// </summary>
		/// <param name="Id">id from ajax call</param>
		/// <returns></returns>
		public JsonResult GetCarForEdit(int Id)
		{
			var carForEdit = carManager.GetCarByCarID(Id);
			if (carForEdit == null)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { success = true, carForEdit }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Gets an object from ajax call, edits it and saves changes to db
		/// </summary>
		/// <param name="car">input car object</param>
		/// <returns></returns>
		public JsonResult EditCar(CarDTO car)
		{
			int userId = SessionUser.Id;
			IEnumerable<CarDTO> DriversCars;
			CarModelValidator validator = new CarModelValidator();
			var checkedCar = validator.Validate(car, ruleSet: "EditThisCar");
			if (!checkedCar.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var newCar = carManager.EditCar(car);
				if (newCar == null)
				{
					return Json(new { success = false }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					DriversCars = carManager.getCarsByUserID(userId);
					return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
				}
			}
		}
		/// <summary>
		/// Gets specific id`s from ajax call and interpret them as values of specific object`s properties.
		/// </summary>
		/// <param name="CarId">id of a car object</param>
		/// <param name="DriverId">Id of a driver</param>
		/// <returns></returns>
		public JsonResult GiveCar(int CarId, int DriverId)
		{
			var statusMessage = carManager.GiveAwayCar(CarId, DriverId);
			if (statusMessage == "Error")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Gets specific id`s from ajax call and interpret them as values of specific object`s properties.
		/// </summary>
		/// <param name="CarId">olololo</param>
		/// <param name="RealOwnerId">ololololo</param>
		/// <returns></returns>
		public JsonResult ReturnCar(int CarId, int RealOwnerId)
		{
			var statusMessage = carManager.GiveAwayCar(CarId, RealOwnerId);
			if (statusMessage == "Error")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Gets specific id form ajax call and sets Car object status, which id matching received id value. 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public JsonResult SetCarStatus(int Id)
		{
			int userId = SessionUser.Id;
			var statusMessage = carManager.ChangeCarToMain(Id, userId);
			if (statusMessage == "Error")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}  
#endif 
	#endregion

#region ExperCar
		#if EXPERIMENTAL
		/// <summary>
		/// Action method, returns main view and populates it with list of objects !!!
		/// </summary>
		/// <returns></returns>
		public ActionResult CarPark()
		{
			AutoParkViewModel CarsViewModel = new AutoParkViewModel();

			if (SessionUser == null || SessionUser.RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				int userId = SessionUser.Id;
				CarsViewModel.Cars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				CarsViewModel.Drivers = ApiRequestHelper.GetById<List<UserDTO>>("Cars", "GetDriversBy", userId).Data as List<UserDTO>;
				//CarsViewModel.Cars = carManager.getCarsByUserID(userId).ToList();
				//CarsViewModel.Drivers = userManager.GetDriversExceptCurrent(userId);
				return View(CarsViewModel);
			}
		}
		//!
		public JsonResult SortBy(string parameter)
		{
			IEnumerable<CarDTO> DriversCars;
			int userId = SessionUser.Id;
			//DriversCars = carManager.getCarsByUserID(userId, parameter).ToList();
			//return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId, parameter).Data as List<CarDTO>; //!!
			return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Gets an object from ajax call and adds it to database
		/// </summary>
		/// <param name="car">object from ajax call</param>
		/// <returns></returns>
		public JsonResult AddNewCar(CarDTO car)
		{
			IEnumerable<CarDTO> DriversCars;
			int userId = SessionUser.Id;
			CarModelValidator validator = new CarModelValidator();
			var checkedCar = validator.Validate(car, ruleSet: "AddNewCar");
			if (!checkedCar.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var response = ApiRequestHelper.PostObject<CarDTO>("Cars", "PostCar", car);
				DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				//carManager.addCar(car);
				//DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Get an id from ajax call and deletes a car object with current id
		/// </summary>
		/// <param name="Id">Id from ajax call</param>
		/// <returns></returns>
		public JsonResult DeleteCar(int Id)
		{
			var response = ApiRequestHelper.DeleteById<CarDTO>("Cars", "DeleteCarBy", Id);
			//var statusMessage = carManager.deleteCarByID(Id);
			if (response.StatusCode.ToString() == "OK")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				//IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Get an id from ajax call and returns a car object with current id.
		/// </summary>
		/// <param name="Id">id from ajax call</param>
		/// <returns></returns>
		public JsonResult GetCarForEdit(int Id)
		{

			var carForEdit = ApiRequestHelper.GetById<CarDTO>("Cars", "GetCarBy", Id);
			//var carForEdit = carManager.GetCarByCarID(Id);
			if (carForEdit.StatusCode.ToString() != "OK")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { success = true, carForEdit }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Gets an object from ajax call, edits it and saves changes to db
		/// </summary>
		/// <param name="car">input car object</param>
		/// <returns></returns>
		public JsonResult EditCar(CarDTO car)
		{
			int userId = SessionUser.Id;
			IEnumerable<CarDTO> DriversCars;
			CarModelValidator validator = new CarModelValidator();
			var checkedCar = validator.Validate(car, ruleSet: "EditThisCar");
			if (!checkedCar.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var newCar = ApiRequestHelper.PostObject<CarDTO>("Cars","PutCar", car);
				//var newCar = carManager.EditCar(car);
				if (newCar.StatusCode.ToString() != "Created")
				{
					return Json(new { success = false }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
					//DriversCars = carManager.getCarsByUserID(userId);
					return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
				}
			}
		}

		/// <summary>
		/// Gets specific id`s from ajax call and interpret them as values of specific object`s properties.
		/// </summary>
		/// <param name="CarId">id of a car object</param>
		/// <param name="DriverId">Id of a driver</param>
		/// <returns></returns>
		public JsonResult GiveCar(int CarId, int DriverId)
		{
			var statusMessage = ApiRequestHelper.PutById<CarDTO>("Cars", "GiveAwayCar", CarId, DriverId);
			//var statusMessage = carManager.GiveAwayCar(CarId, DriverId);
			if (statusMessage.StatusCode.ToString() != "OK")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				//IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Gets specific id`s from ajax call and interpret them as values of specific object`s properties.
		/// </summary>
		/// <param name="CarId">olololo</param>
		/// <param name="RealOwnerId">ololololo</param>
		/// <returns></returns>
		public JsonResult ReturnCar(int CarId, int RealOwnerId)
		{
			//var statusMessage = carManager.GiveAwayCar(CarId, RealOwnerId);
			var statusMessage = ApiRequestHelper.PutById<CarDTO>("Cars", "GiveAwayCar", CarId, RealOwnerId);
			if (statusMessage.StatusCode.ToString() != "OK")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				int userId = SessionUser.Id;
				IEnumerable<CarDTO> DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				//IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Gets specific id form ajax call and sets Car object status, which id matching received id value. 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public JsonResult SetCarStatus(int Id)
		{
			int userId = SessionUser.Id;
			var statusMessage = ApiRequestHelper.PutById<CarDTO>("Cars", "SetCarStatus", Id, userId);
			//var statusMessage = carManager.ChangeCarToMain(Id, userId);
			if (statusMessage.StatusCode.ToString() != "Created")
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				IEnumerable<CarDTO> DriversCars = ApiRequestHelper.GetById<List<CarDTO>>("Cars", "GetCarsBy", userId).Data as List<CarDTO>;
				return Json(new { success = true, DriversCars }, JsonRequestBehavior.AllowGet);
			}
		}  

#endif 
	#endregion
	}
}
