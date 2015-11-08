﻿using BAL.Manager;
using Common.Enum;
using DAL;
using Common.Helpers;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using MainSaite.Models;
using FluentValidation;
using BAL.Tools;


namespace MainSaite.Controllers
{
	public class SettingsController : BaseController
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

		public ActionResult Index()
		{
			return View();
		}


        public PartialViewResult PartialUsersTable()
        {
            var users = userManager.GetUsers();
            return PartialView("_UserTable", users);
        }

        [HttpPost]
        public PartialViewResult PartialUsersTable(UserDTO user)
        {
            ChangeMenu(user);

            var users = userManager.GetUsers();

            return PartialView("_UserTable", users);
        }

		public ActionResult UsersMenu()
		{
			var users = userManager.GetUsers();

			return View(users);
		}

		public PartialViewResult ChangeMenu(int id = 0)
		{

			var user = userManager.GetById(id);
			return PartialView("_EditUser",user);
		}
		[HttpPost]
		public ActionResult ChangeMenu(UserDTO user)
		{
			if (ModelState.IsValid)
			{
				///Think about this 3 strings
				userManager.ChangeUserParameters(user);
				return RedirectToAction("UsersMenu");
			}

			return View(user);
		}

		public ActionResult DistrictEditor()
		{
			var districts = districtManager.getDistricts();
			return View("DistrictEditor", districts);
		}

		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to create new district entry
		/// </summary>
		/// <param name="district">new object</param>
		/// <returns></returns>
		public JsonResult AddDistrict(District district)
		{
			DistrictModelValidator validator = new DistrictModelValidator();
			var checkedDistirct = validator.Validate(district, ruleSet: "AddDistrict");
			if (!checkedDistirct.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				string statusMessage = districtManager.addDistrict(district.Name);
				if (statusMessage == "Error") { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
				else
				{
					var districts = districtManager.getDistricts();
					return Json(new { success = true, districts }, JsonRequestBehavior.AllowGet);
				}
			}
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to delete specific entry from a database
		/// </summary>
		/// <param name="Id">represents an id  of an entry which need to be deleted</param>
		/// <returns></returns>
		public JsonResult DeleteDistrict(District district)
		{
			var checkObject = districtManager.SetDistrictDeleted(district.Id, district.Name);
			if (checkObject == null) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
			var districts = districtManager.getDistricts();
			return Json(new { success = true, districts }, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to edit specific entry in a database
		/// </summary>
		/// <param name="district">objects that contains all data needed for editing information</param>
		/// <returns></returns>
		public JsonResult EditDistrict(District district)
		{
			DistrictModelValidator validator = new DistrictModelValidator();
			var checkedDistirct = validator.Validate(district, ruleSet: "EditDistrict");
			if (!checkedDistirct.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				districtManager.EditDistrict(district);
				var districts = districtManager.getDistricts();
				return Json(new { success = true, districts }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to render list of deleted districts in modal window
		/// </summary>
		/// <returns></returns>
		public JsonResult DeletedDistricts()
		{
			var deletedDistricts = districtManager.getDeletedDistricts();
			return Json(new { deletedDistricts }, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to restore specific deleted district
		/// </summary>
		/// <param name="district">object that contains all the data needed to find and restore district</param>
		/// <returns></returns>
		public JsonResult RestoreDistrict(int id)
		{
			var checkObject = districtManager.RestoreDistrict(id);
			if (checkObject == null) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
			else
			{
				var deletedDistricts = districtManager.getDeletedDistricts();
				var workingDistricts = districtManager.getDistricts();
				return Json(new { success = true, deletedDistricts, workingDistricts }, JsonRequestBehavior.AllowGet); 
			}
		}

		public JsonResult GetAvailableDistricts()
		{
			var districts = districtManager.getDistricts();
			return Json(districts, JsonRequestBehavior.AllowGet);
		}

		// Nick: Car info settings
		//public ActionResult CarEditor()
		//{
		//	if (null == SessionUser || SessionUser.RoleId != (int)AvailableRoles.Driver)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.NotFound);
		//	}
		//	else
		//	{
		//		int userId = SessionUser.Id;
		//		return View(carManager.getCarsByUserID(userId));
		//	}
		//}
		//// GET:
		//public ActionResult CarCreate()
		//{
		//	return View();
		//}

		//// POST:
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult CarCreate(CarDTO car)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			carManager.addCar(car);
		//			return RedirectToAction("CarEditor");
		//		}
		//	}
		//	catch (DataException)
		//	{
		//		ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
		//	}
		//	return RedirectToAction("CarEditor");
		//}

		//public ActionResult CarDetails(int id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	var carID = carManager.GetCarByCarID(id);
		//	if (carID == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(carID);
		//}
		//// GET: 
		//public ActionResult CarDelete(int id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	var carForDelete = carManager.GetCarByCarID(id);
		//	if (carForDelete == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(carForDelete);
		//}
		//// POST:
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult CarDelete(CarDTO car)
		//{
		//	try
		//	{
		//		carManager.deleteCarByID(car.Id);
		//	}
		//	catch (DataException)
		//	{
		//		return RedirectToAction("CarDelete");
		//	}
		//	return RedirectToAction("CarEditor");
		//}
		//// GET: 
		//public ActionResult CarEdit(int id)
		//{
		//	//if (id == null)
		//	//{
		//	//	return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	//}
		//	var carID = carManager.GetCarByCarID(id);
		//	if (carID == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(carID);
		//}

		//// POST:
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult CarEdit(CarDTO car)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		carManager.EditCar(car);
		//		return RedirectToAction("CarEditor");
		//	}
		//	return RedirectToAction("CarEditor");
		//}

		//public ActionResult ChangeCarUser(int id)
		//{
		//	CarChangeUser model = new CarChangeUser();
		//	model.Car = carManager.GetCarByCarID(id);
		//	model.Drivers = userManager.GetDrivers().Where(x => x.Id != SessionUser.Id).ToList();
		//	return View(model);
		//}
		//[HttpPost]
		//public ActionResult ChangeCarUser(CarDTO car)
		//{
		//	carManager.EditCar(car);
		//	return RedirectToAction("CarEditor");
		//}

		//public ActionResult ReturnCarBack(int id)
		//{
		//	var model = carManager.GetCarByCarID(id);
		//	return View(model);
		//}

		//[HttpPost]
		//public ActionResult ReturnCarBack(CarDTO car)
		//{
		//	car.UserId = car.OwnerId;
		//	carManager.EditCar(car);
		//	return RedirectToAction("CarEditor");
		//}

		public ActionResult SetVIPStatus()
		{
			return View(userManager.GetVIPClients());
		}


		public ActionResult SetVIPUser(VIPClientDTO client)
		{
			userManager.SetVIPStatus(client.UserId);
			return RedirectToAction("SetVIPStatus");
		}

		public ActionResult DeleteVIPUser(VIPClientDTO client)
		{
			userManager.deleteVIPById(client.Id);
			return RedirectToAction("SetVIPStatus");
		}

	}
}
