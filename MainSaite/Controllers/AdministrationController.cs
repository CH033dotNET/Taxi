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
			if (!ModelState.IsValid)
			{
				return View();
			}
			else if (!userManager.IfUserNameExists(user.UserName) && !userManager.IfEmailExists(user.Email))
			{
				userManager.SetRoleId(user);
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
