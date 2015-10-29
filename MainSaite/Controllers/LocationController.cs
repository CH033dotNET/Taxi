using BAL.Manager;
using Model.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using DAL.Interface;

namespace MainSaite.Controllers
{
	public class LocationController : BaseController
	{
		private IUnitOfWork uOW;
		private ILocationManager locationManager;
		private IDistrictManager districtManager;

		public LocationController(ILocationManager locationManager, IUnitOfWork uOW, IDistrictManager districtManager)
		{
			this.locationManager = locationManager;
			this.uOW = uOW;
			this.districtManager = districtManager;
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
			LocationDTO location = locationManager.GetByUserId(user.Id);
			if (location != null)
			{
				int districtId = location.DistrictId;
				District district = districtManager.getById(districtId);
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

			LocationDTO local = locationManager.GetByUserId(user.Id);
			if (local != null)
			{
				local.DistrictId = Id;
				locationManager.UpdateLocation(local);
				return RedirectToAction("Index", "Driver");
			}
			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = user.Id,
					DistrictId = Id
				};
				locationManager.AddLocation(district);
				return RedirectToAction("Index", "Driver");
			}
		}

	}
}
