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
						if (userManager.IsUserNameCorrect(user.UserName))
						{
							userManager.InsertUser(user);
							
							ValidPerson(user);
							
							return RedirectToAction("Index", "Home");
						}
						else ModelState.AddModelError("", "Login syntax error");
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
				var currentUser = userManager.GetByUserName(user.UserName, user.Password);
				
				if (currentUser != null)
				{
					ValidPerson(currentUser);
					Session["User"] = userManager.GetByUserName(user.UserName, user.Password);
					return RedirectToAction("Index", "Home");
				}
				else ModelState.AddModelError("", "Wrong password or login");
			}

			return View();			
        }

		public ActionResult LogOut()
		{
			Session["User"] = null;
			return RedirectToAction("Index", "Home");
		}

	    public void ValidPerson( UserDTO user)
	    {
			var currentUser = userManager.GetByUserName(user.UserName, user.Password);
			var currentPerson = personManager.GetPersonByUserId(currentUser.Id);
			if (currentPerson == null || currentPerson.ImageName == null)
			{
				currentPerson =
					personManager.InsertPerson(new PersonDTO() { UserId = currentUser.Id, ImageName = "item_0_profile.jpg" });
				currentPerson.User = currentUser;
			} 
	    }
    }
}
