using BAL.Manager;
using Common.Enum;
using DAL;
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
	public class DriverController : BaseController
	{
		
		
		public ActionResult Index()
		{
			if (null == Session["User"] || ((UserDTO)Session["User"]).RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				return View();
			}
		}
		public ActionResult DistrictPart()
		{
			ViewBag.Districts = locationManager.GetDriverDistrictInfo(); ;
			return PartialView(carManager.GetWorkingDrivers());
		}

		public JsonResult WorkStateChange(int Id, string Latitude, string Longitude, string Accuracy)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					CoordinatesDTO coordinates;
					coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					coordinatesManager.AddCoordinates(coordinates);
				}
				carManager.StartWorkEvent(Id);

				return Json(true);

			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		public JsonResult WorkStateEnded(int Id, string Latitude, string Longitude, string Accuracy)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					CoordinatesDTO coordinates;
					coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					coordinatesManager.AddCoordinates(coordinates);
				}
				carManager.EndAllCurrentUserShifts(Id);
				//carManager.EndWorkShiftEvent(user.Id);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
    }
}
