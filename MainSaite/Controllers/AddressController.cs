﻿using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class AddressController : BaseController
    {
		private IAddressManager addressmanager;
		public AddressController(IAddressManager addressmanager)
		{
			this.addressmanager = addressmanager;
		}

        public ActionResult Index()
        {
			var addressList = addressmanager.GetAddresses();
			return View(addressList);
        }

        [HttpGet]
        public ActionResult CreateAddress()
        {
            return View();
        }
        [HttpPost]
         public ActionResult CreateAddress(AddressDTO address) 
        {
            addressmanager.AddAddress(address);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteAddress(int id)
        {

            AddressDTO address = addressmanager.GetById(id);

            return View(address);
        }
        [HttpPost, ActionName("DeleteAddress")]
        public ActionResult DeleteAdd(int id)
        { 
            addressmanager.DeleteAddress(id);
            return RedirectToAction("Index");
        }
     
        [HttpGet]
        public ActionResult EditAddress(int id)
        {
           
            AddressDTO address = addressmanager.GetById(id);

            return View(address);
        }
        [HttpPost]
        public ActionResult EditAddress(AddressDTO address)
        {
            addressmanager.UpdateAddress(address);
            return RedirectToAction("Index");
        }
    }
}
