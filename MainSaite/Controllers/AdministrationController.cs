using BAL.Manager;
using DAL;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainSaite.Models;

namespace MainSaite.Controllers
{
	public class AdministrationController : BaseController
	{
		//
		// GET: /Administration/


		public ActionResult AddUser()
		{
			//var user = mainContext.Users.;
			UserDTO user = new UserDTO();

			return View(user);
		}

		[HttpPost]
		public ActionResult AddUser(UserDTO user)
		{
			List<string> msgs = new List<string>();
			if (userManager.UserValidation(user, msgs))
			{
				if (!userManager.IfUserNameExists(user.UserName) && !userManager.IfEmailExists(user.Email))
				{
					userManager.InsertUser(user);
					return RedirectToAction("UsersMenu", "Settings");
				}
				else
				{
					ModelState.Clear();
					ModelState.AddModelError("", Resources.Resource.LoginEmailExist);
					return View();
				}
			}
			else
			{
				ModelState.Clear();
				foreach (string msg in msgs)
				{
					ModelState.AddModelError("", msg);
				}
				return View();
			}
		}

		//Nick //ASIX
		public ActionResult ViewWorkShifts()
		{
			/*//var popa = uOW.WorkshiftHistoryRepo.All.GroupBy(x => x.DriverId).Select(z => z.Max(y => y.WorkStarted)).ToList();
			var drvs = uOW.CoordinatesHistoryRepo.All
				.GroupBy(x => x.UserId)
				.Select(grp => grp.OrderByDescending(apo => apo.AddedTime).FirstOrDefault()).Join(
				uOW.UserRepo.All, outr => outr.UserId, inr => inr.Id, (myinr, myotr) => new DriverLocation() { id = myotr.Id, addedtime = myinr.AddedTime, latitude = myinr.Latitude, longitude = myinr.Longitude, name = myotr.UserName }).ToList();*/
			//var workingDrivers = carManager.GetWorkingDrivers();
			return View();
		}



		public JsonResult GetLoc()
		{
			var drvs = uOW.CoordinatesHistoryRepo.All
				.GroupBy(x => x.UserId)
				.Select(grp => grp.OrderByDescending(apo => apo.AddedTime).FirstOrDefault()).Join(
				uOW.UserRepo.All, outr => outr.UserId, inr => inr.Id, (myinr, myotr) => new DriverLocation() {
					id = myotr.Id, addedtime = myinr.AddedTime, latitude = myinr.Latitude, longitude = myinr.Longitude, name = myotr.UserName }).ToArray();
			return Json(drvs, JsonRequestBehavior.AllowGet);
		}
	}
}
