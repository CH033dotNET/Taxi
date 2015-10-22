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
			return View();
		}



		public JsonResult GetLoc()
		{
			var driversForOperators = 
				uOW.CoordinatesHistoryRepo.All //all coordinates
				.GroupBy(coordinates => coordinates.UserId) //grouping all coordinates for each user
				.Select(group => group.OrderByDescending(coordinates => coordinates.AddedTime).FirstOrDefault()) //select Latest coordinates from each group
				.Join(uOW.UserRepo.All, //add user data to coordinates
				coordinates => coordinates.UserId, user => user.Id, 
				(coordinates, user) => new //create new model whith nesessary field
				{
					id = user.Id,
					addedtime = coordinates.AddedTime,
					latitude = coordinates.Latitude,
					longitude = coordinates.Longitude,
					name = user.UserName
				})
				.Join(uOW.WorkshiftHistoryRepo.All//join shifts
				.Where(shift => shift.WorkStarted != null & shift.WorkEnded == null)//select only current shifts
				, driver => driver.id, shift => shift.DriverId, (driver, shift) => new DriverLocation()
				{
					id = driver.id,
					updateTime = driver.addedtime,
					latitude = driver.latitude,
					longitude = driver.longitude,
					name = driver.name,
					startedTime = shift.WorkStarted
				}
				) // select drivers which now working
				.ToArray();
            return Json(driversForOperators, JsonRequestBehavior.AllowGet);
		}
	}
}
