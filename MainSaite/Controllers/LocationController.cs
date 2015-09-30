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
	public class LocationController : BaseController
	{
		//
		// GET: /Localization/
		LocationManager locationmanager;
		DistrictManager districtmanager;
		public LocationController()
		{
			locationmanager = new LocationManager(base.uOW);
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
			LocationDTO location = locationmanager.GetByUserId(user.Id);
			if (location != null)
			{
				int districtId = location.DistrictId;
				District district = districtmanager.getById(districtId);
				ViewBag.District = district;
			}
			return PartialView();
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

			LocationDTO local = locationmanager.GetByUserId(user.Id);
			if (local != null)
			{
				local.DistrictId = Id;
				locationmanager.UpdateLocation(local);
				return RedirectToAction("Index", "Driver");
			}
			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = user.Id,
					DistrictId = Id
				};
				locationmanager.AddLocation(district);
				return RedirectToAction("Index", "Driver");
			}
		}

	}
}
