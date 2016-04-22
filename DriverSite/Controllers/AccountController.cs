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
                SessionUser = ApiRequestHelper.postData<UserDTO, LoginModel>("Account", "getUser", user).Data as UserDTO;
                if (SessionUser != null)
				{
                    if ((SessionUser.Role.Id == (int)Common.Enum.AvailableRoles.Driver) ||
                       (SessionUser.Role.Id == (int)Common.Enum.AvailableRoles.FreeDriver)) {
                        return RedirectToAction("Index", "Home");
                    } else {
                        return LogOut();
                    }
				}
				else ModelState.AddModelError("", "Wrong password or login");
			}

			return View();			
        }

		public ActionResult LogOut()
		{
            SessionUser = null;
			return RedirectToAction("Index", "Home");
		}
    }
}
