using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Manager;
using DAL;

namespace MainSaite.Controllers
{
	public class UserController : Controller
	{
		//UserManager userManager;

		//public UserController(UserManager userManager)
		//{
		//    this.userManager = userManager;
		//}

		//public UserController()
		//{
		//    UnitOfWork uow = new UnitOfWork();
		//    userManager = new UserManager(uow);
		//}

		public ActionResult Index()
		{
			//var users = userManager.GetUsers();
			//ViewBag.Users = users;
			return View();
		}

	}
}
