using BAL.Manager;
using Model.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace MainSaite.Controllers
{
	public class LocalizationController : BaseController
	{
		//
		// GET: /Localization/
		LocalizationManager localmanager;
		DistrictManager districtmanager;
		public LocalizationController()
		{
			localmanager = new LocalizationManager(base.uOW);
			districtmanager = new DistrictManager(base.uOW);
		}

		[HttpGet]
		public ActionResult EditLocation()
		{
			var user = Session["User"] as Model.DTO.UserDTO;
			if (user == null)
			{
				return RedirectToRoute(new
				{
					controller = "Home",
					action = "Index"
				});
			}

			var listDistricts = uOW.DistrictRepo.Get().ToList();
			ViewBag.Districts = listDistricts;

			LocalizationDTO localization = localmanager.GetByUserId(user.Id);
			if (localization == null)
				return RedirectToAction("CreateLocation");
			else
			{
				int districtId = localization.DistrictId;

				District district = districtmanager.getById(districtId);
				string districtName = district.Name;
				ViewBag.DistrictName = districtName;
				return View();
			}
		}

		[HttpPost]
		public ActionResult EditLocation(int Id)
		{
			var user = Session["User"] as Model.DTO.UserDTO;
			if (user == null)
			{
				return RedirectToRoute(new
				{
					controller = "Home",
					action = "Index"
				});
			}

			LocalizationDTO local = localmanager.GetByUserId(user.Id);
			local.DistrictId = Id;
			localmanager.UpdateLocalization(local);
			return RedirectToAction("Index", "Home"); ;
		}

		[HttpGet]
		public ActionResult CreateLocation()
		{
			var listDistricts = uOW.DistrictRepo.Get().ToList();
			ViewBag.Districts = listDistricts;
			return View();
		}
		[HttpPost]
		public ActionResult CreateLocation(string Name)
		{
			int districtId = uOW.DistrictRepo.Get().Where(x => x.Name == Name).First().Id;
			LocalizationDTO district = new LocalizationDTO()
			{
				UserId = (Session["User"] as Model.DTO.UserDTO).Id,
				DistrictId = districtId
			};
			localmanager.AddLoc(district);
			return RedirectToAction("Index", "Home");
		}
	}
}
