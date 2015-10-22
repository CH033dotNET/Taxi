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

namespace MainSaite.Controllers
{
    public class CarsController : BaseController
    {
		//
		// GET: /Cars/

		public ActionResult CarPark()
		{
			AutoParkViewModel CarsViewModel = new AutoParkViewModel();
			if (session.User == null || session.User.RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				int userId = session.User.Id;
				CarsViewModel.Cars = carManager.getCarsByUserID(userId).ToList();
				CarsViewModel.Drivers = userManager.GetDrivers().Where(x => x.Id != session.User.Id).ToList(); //!!!!!
				//var list = carManager.getCarsByUserID(userId).ToList();
				return View(CarsViewModel);
			}
		}

		public JsonResult AddNewCar(CarDTO car)
		{
			IEnumerable<CarDTO> DriversCars;
			int userId = session.User.Id;
			try
			{
				if (ModelState.IsValid)
				{
					carManager.addCar(car);
					DriversCars = carManager.getCarsByUserID(userId);
					return Json(DriversCars, JsonRequestBehavior.AllowGet);
				}
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			DriversCars = carManager.getCarsByUserID(userId);
			return Json(DriversCars, JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetCarDetails()
		{
			return Json(null, JsonRequestBehavior.AllowGet);
		}
		public JsonResult DeleteCar(int Id)
		{
			carManager.deleteCarByID(Id);
			int userId = session.User.Id;
			IEnumerable<CarDTO> DriversCars = carManager.getCarsByUserID(userId);
			return Json(DriversCars, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCarForEdit(int Id)
		{
			var carForEdit = carManager.GetCarByCarID(Id);
			return Json(carForEdit,JsonRequestBehavior.AllowGet);
		}
		public JsonResult EditCar(CarDTO car)
		{
			carManager.EditCar(car);
			int userId = session.User.Id;
			return Json(carManager.getCarsByUserID(userId), JsonRequestBehavior.AllowGet);
		}
		public JsonResult GiveCar(int CarId, int DriverId)
		{
			carManager.GiveAwayCar(CarId, DriverId);
			int userId = session.User.Id;
			return Json(carManager.getCarsByUserID(userId), JsonRequestBehavior.AllowGet);
		}
		public JsonResult ReturnCar(int CarId, int RealOwnerId)
		{
			carManager.GiveAwayCar(CarId, RealOwnerId);
			int userId = session.User.Id;
			return Json(carManager.getCarsByUserID(userId), JsonRequestBehavior.AllowGet);
		}
    }
}
