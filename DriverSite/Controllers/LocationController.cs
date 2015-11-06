using Model.DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace DriverSite.Controllers
{
	public class LocationController : BaseController
	{
        readonly string controller = "Location";
		[HttpGet]
		public ActionResult EditLocation()
		{
			var user = Session["User"] as Model.DTO.UserDTO;
			if (user == null)
			{
                return RedirectToAction("Index", "Home");
			} 
            var listDistricts = ApiRequestHelper.Get<List<District>>(controller, "getDistricts").Data;
            //var listDistricts = districtManager.getDistricts();
			ViewBag.Districts = listDistricts;
            LocationDTO location = ApiRequestHelper.GetById<LocationDTO>(controller, "GetByUserId", user.Id).Data;
			//LocationDTO location = locationManager.GetByUserId(user.Id);
			if (location != null)
			{
				int districtId = location.DistrictId;
                District district = ApiRequestHelper.GetById<District>(controller, "getById", districtId).Data;
				//District district = districtManager.getById(districtId);
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
                return RedirectToAction("Index", "Home");
			}
            
            LocationDTO local = ApiRequestHelper.GetById<LocationDTO>(controller, "GetByUserId", user.Id).Data;
			//LocationDTO local = locationManager.GetByUserId(user.Id);
			if (local != null)
			{
				local.DistrictId = Id;
                ApiRequestHelper.postData<LocationDTO>(controller, "UpdateLocation", local);
				//locationManager.UpdateLocation(local);
                return RedirectToAction("Index", "Home");
			}
			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = user.Id,
					DistrictId = Id
				};

                ApiRequestHelper.postData<LocationDTO>(controller, "AddLocation", district);
				//locationManager.AddLocation(district);
                return RedirectToAction("Index", "Home");
			}
		}

	}
}
