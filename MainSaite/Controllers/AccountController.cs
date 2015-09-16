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
		public ActionResult Registration(UserDTO user)
		{
			if (ModelState.IsValid)
			{
				if (!userManager.IfUserNameExists(user.UserName))
				{
					userManager.InsertUser(user);
				}

				else ModelState.AddModelError("", "User is already exist");
			}
			return View();
		}

        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(UserDTO user)
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

			return View();
        }
    }
}
