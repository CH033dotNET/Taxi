using Common.Enum;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using Model.DB;

namespace DriverSite.Controllers
{
    public class AccountController : BaseController
    {

        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(LoginModel user)
		{
			if (ModelState.IsValid)
			{
                SessionUser = ApiRequestHelper.Get<UserDTO, string, string>("Account", "getUser", user.UserName, user.Password).Data as UserDTO;
                if (SessionUser != null)
				{
                    SessionPerson = ApiRequestHelper.GetById<PersonDTO>("Account", "getPerson", SessionUser.Id).Data as PersonDTO;
					return RedirectToAction("Index", "Home");
				}
				else ModelState.AddModelError("", "Wrong password or login");
			}

			return View();			
        }

		public ActionResult LogOut()
		{
            SessionUser = null;
            SessionPerson = null;
			return RedirectToAction("Index", "Home");
		}
    }
}
