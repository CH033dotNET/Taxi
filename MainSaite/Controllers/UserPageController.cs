﻿using BAL.Interfaces;
using BAL.Manager;
using MainSaite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class UserPageController : BaseController
    {
		private IAddressManager addressmanager;
		private IPersonManager personManager;
		private IOrderManager orderManager;
		public UserPageController(IAddressManager addressmanager, IPersonManager personManager, IOrderManager orderManager)
		{
			this.addressmanager = addressmanager;
			this.personManager = personManager;
			this.orderManager = orderManager;
		}

        public ActionResult Index()
        {
            UserPagePhoneModel model = new UserPagePhoneModel();
            if (SessionUser != null)
            { 
                model.addresses = addressmanager.GetAddressesForUser(SessionUser.Id).ToList();
                model.person = personManager.GetPersonByUserId(SessionUser.Id);
                model.DroPlaces = orderManager.GetOrdersByUserId(SessionUser.Id).Select(x => x.DropPlace).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Index(PersonDTO person)
         {
            var jsonOk = new { success = true };
            var jsonNeOk = new { success = false, person = person };
            if (!ModelState.IsValid)
                return Json(jsonNeOk);

            personManager.UpdatePhoneFMLnames(person);


            return Json(jsonOk);
        }


		public ActionResult Iframe()
		{
			return View();
		}


	    
    }
}
