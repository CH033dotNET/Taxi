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
         
        AddressManager addressmanager;
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
        public ActionResult CreateAdd()
        {
            return View();
        }
        [HttpPost]
         public ActionResult CreateAdd(AddressDTO address) 
        {
          
            addressmanager.AddAddress(address);
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {

            AddressDTO address = addressmanager.GetById(id);

            return View(address);
        }
        [HttpPost, ActionName("Delete")]
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
        public ActionResult EditAdd(int id)
        {
           
            AddressDTO address = addressmanager.GetById(id);

            return View(address);
        }
        [HttpPost]
        public ActionResult EditAdd(AddressDTO address)
        {
            addressmanager.UpdateAddress(address);
            return RedirectToAction("Index");
        }
    }
}
