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
		private IUserManager userManager;
		private IDriverManager driverManager;

		public AdministrationController(IUserManager userManager, IDriverManager driverManager)
		{
			this.userManager = userManager;
			this.driverManager = driverManager;
		}
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

		public ActionResult ViewWorkShifts()
		{
			return View();
		}


		public JsonResult GetLoc()
		{				
            return Json(driverManager.GetFullLocations(), JsonRequestBehavior.AllowGet);
		}
	}
}
