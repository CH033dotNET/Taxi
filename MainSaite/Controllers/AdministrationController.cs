using BAL.Manager;
using DAL;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class AdministrationController : BaseController
    {
        //
        // GET: /Administration/

		UnitOfWork unit = new UnitOfWork();
		UserManager userManager = null;

		public AdministrationController()
		{
			userManager = new UserManager(unit);
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
			if (!userManager.IfUserNameExists(user.UserName) && !userManager.IfEmailExists(user.Email))
			{
				userManager.InsertUser(user);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return View();
			}
		}

    }
}
