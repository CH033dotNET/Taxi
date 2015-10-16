using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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


		public JsonResult WorkStateChange(int Id, string Latitude, string Longitude, string Accuracy, string TimeStart)
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
				carManager.StartWorkEvent(Id, TimeStart);

				return Json(true);

			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		public JsonResult WorkStateEnded(int Id, string Latitude, string Longitude, string Accuracy, string TimeStop)
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

				var message = carManager.EndAllCurrentUserShifts(Id, TimeStop);
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
		public ActionResult GetArticle(string timeStart, string timeStop)
		{

			string format = "d HH:mm:ss tt";
			try
			{
				string s = "2011/03/21 13:26";

				DateTime dt = DateTime.ParseExact(timeStart, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

				string windowsTime = "2/21/2009 10:35 PM";

				var time = DateTime.Parse(windowsTime);
				Console.WriteLine(time);
				DateTime result = DateTime.Parse(timeStart);
			}
			catch (FormatException)
			{
				Console.WriteLine("{0} is not in the correct format.", timeStart);

			}

			var dateString = "15/06/2008 08:30";
			format = "g";
			var provider = new CultureInfo("fr-FR");
			try
			{
				var result = DateTime.ParseExact(dateString, format, provider);
				Console.WriteLine("{0} converts to {1}.", dateString, result.ToString());
			}
			catch (FormatException)
			{
				Console.WriteLine("{0} is not in the correct format.", dateString);
			}
			return new EmptyResult();
		}
	}
}
