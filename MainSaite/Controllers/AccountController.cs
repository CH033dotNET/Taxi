using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using Model.DB;

namespace MainSaite.Controllers
{
    public class AccountController : BaseController
    {
		
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
				if (!userManager.IfUserNameExists(user.UserName) && !userManager.IfEmailExists(user.Email))
				{
					if (userManager.IsUserNameCorrect(user.UserName))
					{
						userManager.InsertUser(user);

						CheckPerson(user);
						Authentification(new LoginModel() {  UserName = user.UserName, Password = user.Password});
						return RedirectToAction("Index", "User");
					}
					//else ModelState.AddModelError("", "Login syntax error");
				}
				//else ModelState.AddModelError("", "User Name or Email is already exist");
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
					CheckPerson(currentUser);
					SessionUser = currentUser;
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
		/// <summary>
		/// Not support browsers less than IE9
		/// </summary>
		/// <returns></returns>
		public ActionResult IEDisable()
		{
			return View();
		}

	    private void CheckPerson( UserDTO user)
	    {
			var currentUser = userManager.GetByUserName(user.UserName, user.Password);
			var currentPerson = personManager.GetPersonByUserId(currentUser.Id);

			if (currentPerson == null)
			{
				currentPerson =
					personManager.InsertPerson(new PersonDTO() { UserId = currentUser.Id, ImageName = "item_0_profile.jpg" });
				currentPerson.User = currentUser;
			}
			if (!System.IO.File.Exists(Server.MapPath(@"~\Images\") + currentPerson.ImageName))
			{
				personManager.DefaultImage(user.Id);
			}
	    }
    }
}
