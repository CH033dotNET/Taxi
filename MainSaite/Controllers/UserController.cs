using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Manager;
using Model.DTO;

namespace MainSaite.Controllers
{
	public class UserController : BaseController
	{

		UserManager userManager = null;

		public UserController()
		{
			userManager = new UserManager(uOW);
		}

		public ActionResult Index()
		{

			var user = (UserDTO)(Session["User"]);


			if (user != null)
				return View(user);

			return RedirectToAction("Authentification", "Account");

		}

		[HttpPost]
		public ActionResult Index(bool change, string firstName, string lastName, string phone)
		{

			var user = (UserDTO)(Session["User"]);
			string userFirstName = firstName;
			string userLastName = firstName;
			int j;
			if (Int32.TryParse(phone, out j))
				j = 0;

			return View();
		}
	}
}
