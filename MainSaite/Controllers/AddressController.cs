using BAL.Manager;
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
         
        MainContext db = new MainContext();
        public AddressController()
        {
            addressmanager = new AddressManager(base.uOW);
        }
       
        public ActionResult Index()
        {
           var ad = addressmanager.GetAddresses();
            return View(ad);
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
      /*  [HttpGet]
        public ActionResult EditAdd2(AddressDTO address)
        {
            return View(address);
        }*/

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
