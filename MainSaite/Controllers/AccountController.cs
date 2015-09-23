using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Manager;
using Model.DTO;
using Model.DB;

namespace MainSaite.Controllers
{
    public class AccountController : BaseController
    {
		
		UserManager userManager = null;

		public AccountController()
		{
			userManager = new UserManager(uOW);
		}

        //
        // GET: /Acount/

        public ActionResult Registration()
        {
            return View();
			
        }

		[HttpPost]
		public ActionResult Registration(RegistrationModel user)
		{
			if (ModelState.IsValid)
			{
				if (!userManager.IfUserNameExists(user.UserName))
				{
					if (!userManager.IfEmailExists(user.Email))
					{
						userManager.InsertUser(user);
						return RedirectToAction("Index", "Home");
					}
					else ModelState.AddModelError("", "Email is already exist");
				}
				else ModelState.AddModelError("", "User is already exist");
			}
			
			return View(user);
		}

        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(LoginModel user)
		{
			if (ModelState.IsValid)
			{
				if (user != null)
				{
					Session["User"] = userManager.GetByUserName(user.UserName, user.Password);
					return RedirectToAction("Index", "Home");
				}
			}
			else ModelState.AddModelError("", "Wrong password or login");

			return View(user);
        }

		public ActionResult LogOut()
		{
			Session["User"] = null;
			return RedirectToAction("Index", "Home");
		}
    }
}
