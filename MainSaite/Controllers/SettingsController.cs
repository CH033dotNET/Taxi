using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MainSaite.Controllers
{
	public class SettingsController : BaseController
	{
		UserManager userManager;

		//Patraboi varible
		MainContext db = new MainContext();


		public SettingsController()
		{
			userManager = new UserManager(base.uOW);
		}


		public ActionResult Index()
		{
			return View();
		}


		public ActionResult UsersMenu()
		{
			var users = userManager.GetUsers();

			return View(users);
		}

		public ActionResult ChangeMenu(int id = 0)
		{

			var user = userManager.GetById(id);
			if (user == null)
			{
				return HttpNotFound();
			}

			return View(user);
		}
		[HttpPost]
		public ActionResult ChangeMenu(UserDTO user)
		{
			if (ModelState.IsValid)
			{
				///Think about this 3 strings
				userManager.ChangeUserParameters(user);
				return RedirectToAction("UsersMenu");
			}

			return View(user);
		}

		public ActionResult DistrictEditor()
		{
			var distincts = db.Districts.ToList();
			return View("DistrictEditor", distincts);
		}

		[HttpPost]
		public ActionResult DistrictEditor(string Name)
		{
			District a = new District();
			a.Name = Name;
			db.Districts.Add(a);
			db.SaveChanges();
			return RedirectToAction("DistrictEditor");
		}
		
		public ActionResult DeleteDistrict(District a)
		{
			db.Districts.Attach(a);
			db.Districts.Remove(a);
			db.SaveChanges();

			return RedirectToAction("DistrictEditor");
		}
    }
}
