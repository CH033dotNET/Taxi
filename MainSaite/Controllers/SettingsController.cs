﻿using BAL.Manager;
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

		//ASIX
		DistrictManager districtManager;
		MainContext db = new MainContext();//


		public SettingsController()
		{
			userManager = new UserManager(base.uOW);

			//ASIX
			districtManager = new DistrictManager(base.uOW);
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
			return View("DistrictEditor", districtManager.getDistricts());
		}

		[HttpPost]
		public ActionResult DistrictEditor(string Name)
		{
			districtManager.addDistrict(Name);
			return RedirectToAction("DistrictEditor");
		}
		
		public ActionResult DeleteDistrict(District a)
		{
			districtManager.deleteDistrictById(a.Id);
			return RedirectToAction("DistrictEditor");
		}
    }
}
