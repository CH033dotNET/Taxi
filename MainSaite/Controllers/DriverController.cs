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
		public DriverController()
		{
			//Nick
			carManager = new CarManager(base.uOW);
		}
		
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

		public ActionResult WorkStateChange(UserDTO user)
		{
			try
			{

				carManager.StartWorkEvent(user.Id);
				return RedirectToAction("Index");

			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("Index");
		}
		public ActionResult WorkStateEnded(UserDTO user)
		{
			try
			{
				carManager.EndAllCurrentUserShifts(user.Id);
				//carManager.EndWorkShiftEvent(user.Id);
				return RedirectToAction("Index");
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("Index");
		}
    }
}
