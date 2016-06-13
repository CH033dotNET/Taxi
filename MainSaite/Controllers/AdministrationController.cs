using BAL.Interfaces;
using BAL.Manager;
using Common.Enum.DriverEnum;
using MainSaite.Helpers;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class AdministrationController : BaseController
	{
		private IUserManager userManager;
		private IPersonManager personManager;
		private IWorkerStatusManager workerStatusManager;
		private IDriverExManager driverManager;

		public AdministrationController(
			IUserManager userManager,
			IPersonManager personManager,
			IWorkerStatusManager workerStatusManager,
			IDriverExManager driverManager)
		{
			this.userManager = userManager;
			this.personManager = personManager;
			this.workerStatusManager = workerStatusManager;
			this.driverManager = driverManager;
        }

		public ActionResult AddUser()
		{
			UserDTO user = new UserDTO();
			return View(user);
		}

		[AuthFilter(Roles = "Administrator")]
		[HttpPost]
		public ActionResult AddUser(UserDTO user)
		{
            user.Lang = "en-us";
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

		[AuthFilter(Roles = "Administrator")]
		public ActionResult ViewWorkShifts()
		{
			return View();
		}

		[AuthFilter(Roles = "Administrator")]
		public ActionResult Drivers()
		{
            return View(personManager.GetDrivers());
		}

		[HttpPost]
		public JsonResult GetStatuses()
		{
			return Json(workerStatusManager.GetAllStatuses());
		}
	}
}
