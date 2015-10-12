using BAL.Manager;
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


namespace MainSaite.Controllers
{
	public class SettingsController : BaseController
	{

		public ActionResult Index()
		{
			return View();
		}


		public ActionResult UsersMenu()
		{
			var users = userManager.GetUsers();

			return View(users);
		}

		public ActionResult ChangeMenu(int id = 0)
		{

			var user = userManager.GetById(id);
			if (user == null)
			{
				return HttpNotFound();
			}

			return View(user);
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

		public JsonResult AddDistrict(string Name)
		{
			districtManager.addDistrict(Name);
			var districts = districtManager.getDistricts();
			return Json(districts,JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to delete specific entry from a database
		/// </summary>
		/// <param name="Id">represents an id  of an entry which need to be deleted</param>
		/// <returns></returns>
		public JsonResult DeleteDistrict(District district)
		{
			//districtManager.deleteById(Id);
			districtManager.SetDistrictDeleted(district.Id, district.Name);
			var districts = districtManager.getDistricts();
			return Json(districts, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to edit specific entry in a database
		/// </summary>
		/// <param name="district">objects that contains all data needed for editing information</param>
		/// <returns></returns>
		public JsonResult EditDistrict(District district)
		{
			if (ModelState.IsValid)
			{
				districtManager.EditDistrict(district);
				var districts = districtManager.getDistricts();
				return Json(districts, JsonRequestBehavior.AllowGet);
			}
			return Json(null);
		}
		public JsonResult DeletedDistricts()
		{
			var deletedDistricts = districtmanager.getDeletedDistricts();
			return Json(deletedDistricts,JsonRequestBehavior.AllowGet);
		}
		public JsonResult RestoreDistrict(District district)
		{
			districtManager.RestoreDistrict(district.Id);
			var deletedDistricts = districtManager.getDeletedDistricts();
			return Json(deletedDistricts,JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAvailableDistricts()
		{
			var districts = districtManager.getDistricts();
			return Json(districts, JsonRequestBehavior.AllowGet);
		}

		// Nick: Car info settings
		public ActionResult CarEditor()
		{
			if (null == session.User || session.User.RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				int userId = session.User.Id;
				return View(carManager.getCarsByUserID(userId));
			}
		}
		// GET:
		public ActionResult CarCreate()
		{
			return View();
		}

		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarCreate(CarDTO car)
		{
			try
			{
				if (ModelState.IsValid)
				{
					carManager.addCar(car);
					return RedirectToAction("CarEditor");
				}
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("CarEditor");
		}

		public ActionResult CarDetails(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carID = carManager.GetCarByCarID(id);
			if (carID == null)
			{
				return HttpNotFound();
			}
			return View(carID);
		}
		// GET: 
		public ActionResult CarDelete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carForDelete = carManager.GetCarByCarID(id);
			if (carForDelete == null)
			{
				return HttpNotFound();
			}
			return View(carForDelete);
		}
		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarDelete(CarDTO car)
		{
			try
			{
				carManager.deleteCarByID(car.Id);
			}
			catch (DataException)
			{
				return RedirectToAction("CarDelete");
			}
			return RedirectToAction("CarEditor");
		}
		// GET: 
		public ActionResult CarEdit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carID = carManager.GetCarByCarID(id);
			if (carID == null)
			{
				return HttpNotFound();
			}
			return View(carID);
		}

		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarEdit(CarDTO car)
		{
			if (ModelState.IsValid)
			{
				carManager.EditCar(car);
				return RedirectToAction("CarEditor");
			}
			return RedirectToAction("CarEditor");
		}

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
