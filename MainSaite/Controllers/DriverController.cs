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
			ViewBag.Districts = locationManager.GetDriverDistrictInfo();
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
					coordinates.TarifId = 1;
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
					coordinates.TarifId = 1;
					coordinatesManager.AddCoordinates(coordinates);
				}
				var message = carManager.EndAllCurrentUserShifts(Id);
				ViewBag.WorkEndMessage = message;
				//carManager.EndWorkShiftEvent(user.Id);
				if (locationManager.GetByUserId(Id) != null)
					locationManager.DeleteLocation(Id);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		public ActionResult JoinToLocation(int Id)
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
