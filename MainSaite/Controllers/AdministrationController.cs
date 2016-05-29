using BAL.Manager;
using DAL;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainSaite.Models;
using MainSaite.Helpers;

namespace MainSaite.Controllers
{
	public class AdministrationController : BaseController
	{
		private IUserManager userManager;

		public AdministrationController(IUserManager userManager)
		{
			this.userManager = userManager;
		}
		public ActionResult AddUser()
		{
			//var user = mainContext.Users.;
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
		public ActionResult SendMessagesToDrivers() {
			return View();
		}

	}
}
