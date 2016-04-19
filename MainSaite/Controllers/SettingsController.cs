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
			return PartialView("_EditUser", user);
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
		/// Action method that receives parameter from ajax call and uses it for searching entries
		/// Is used only for searching non-deleted districts
		/// </summary>
		/// <param name="parameter">Represents value from ajax call</param>
		/// <returns></returns>
		public JsonResult SearchDistrict(string parameter)
		{
			if (!String.IsNullOrEmpty(parameter))
			{
				var resultDistricts = districtManager.searchDistricts(parameter).ToList();
				return Json(new { success = true, resultDistricts }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var districts = districtManager.getDistricts();
				return Json(new { success = false, districts }, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult SearchDeletedDistrict(string parameter)
		{
			if (!String.IsNullOrEmpty(parameter))
			{
				var resultDistricts = districtManager.searchDeletedDistricts(parameter).ToList();
				return Json(new { success = true, resultDistricts }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var districts = districtManager.getDeletedDistricts();
				return Json(new { success = false, districts }, JsonRequestBehavior.AllowGet);
			}
		}
		/// <summary>
		/// Action method that receives parameter from ajax call and uses it for searching 
		/// and sorting result entries. Is used solely for non-deleted districts.
		/// </summary>
		/// <param name="search"></param>
		/// <param name="sort"></param>
		/// <returns></returns>
		public JsonResult SearchAndSort(string search, string sort)
		{
			if (!String.IsNullOrEmpty(search))
			{
				var resultDistricts = districtManager.searchAndSortDistricts(search, sort).ToList();
				return Json(new { success = true, resultDistricts }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var resultDistricts = districtManager.GetSortedDistricts(sort).ToList();
				return Json(new { success = true, resultDistricts }, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult DeletedSearchAndSort(string search, string sort)
		{
			if (!String.IsNullOrEmpty(search))
			{
				var sortedDeletedDistricts = districtManager.searchAndSortDeletedDistricts(search, sort).ToList();
				return Json(new { success = true, sortedDeletedDistricts }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var sortedDeletedDistricts = districtManager.GetSortedDeletedDistrictsBy(sort).ToList();
				return Json(new { success = true, sortedDeletedDistricts }, JsonRequestBehavior.AllowGet);
			}
		}

#if BETA
		/// <summary>
		/// Action method that receives parameter from ajax call and uses it for sorting entries
		/// in non-deleted list
		/// </summary>
		/// <param name="parameter">Input parameter taht represents data from ajax call</param>
		/// <returns></returns>
		public JsonResult SortDistrictBy(string parameter)
		{
			IEnumerable<District> sortedDistricts;
			sortedDistricts = districtManager.GetSortedDistricts(parameter).ToList();
			return Json(new { success = true, sortedDistricts }, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Action method that receives parameter from ajax call and uses it for sorting entries
		/// in deleted list
		/// </summary>
		/// <param name="parameter">Input parameter taht represents data from ajax call</param>
		/// <returns></returns>
		public JsonResult SortDeletedDistrictBy(string parameter)
		{
			IEnumerable<District> sortedDeletedDistricts;
			sortedDeletedDistricts = districtManager.GetSortedDeletedDistrictsBy(parameter).ToList();
			return Json(new { success = true, sortedDeletedDistricts }, JsonRequestBehavior.AllowGet);
		} 
#endif

		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to create new district entry
		/// </summary>
		/// <param name="district">new object</param>
		/// <returns></returns>

		[HttpPost]
		public JsonResult AddDistrict(DistrictDTO district)
		{
			DistrictModelValidator validator = new DistrictModelValidator();
			var checkedDistirct = validator.Validate(district, ruleSet: "AddDistrict");
			if (!checkedDistirct.IsValid)
			{
				return Json(new { success = false }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var newDistrict = districtManager.addDistrict(district);
				if (newDistrict == null) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
				else
				{
					return Json(new { success = true, district = newDistrict }, JsonRequestBehavior.AllowGet);
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
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}
		/// <summary>
		/// Ajax call from the view sends a data to controller, 
		/// which will be used to edit specific entry in a database
		/// </summary>
		/// <param name="district">objects that contains all data needed for editing information</param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult EditDistrict(DistrictDTO district)
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
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

		public ActionResult SetVIPStatus()
		{
			return View();
		}


		public JsonResult GetVipClients()
		{
			var VipClients = userManager.GetVIPClients();
			return Json(VipClients, JsonRequestBehavior.AllowGet);
		}

		public void SetClientVip(int clientId)
		{
			userManager.SetVIPStatus(clientId);
		}

		public void DelClientVip(int clientId)
		{
			userManager.deleteVIPById(clientId);
		}



	}
}
