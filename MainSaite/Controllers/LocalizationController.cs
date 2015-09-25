using BAL.Manager;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace MainSaite.Controllers
{
    public class LocalizationController : BaseController
    {
        //
        // GET: /Localization/
        LocalizationManager localmanager;
        public LocalizationController()
        {
            localmanager = new LocalizationManager(base.uOW);
        }

        [HttpGet]
        public ActionResult EditLocation()
        {
            var listDistricts = uOW.DistrictRepo.Get().ToList();
            ViewBag.Districts = listDistricts;
            int id = (Session["User"] as Model.DTO.UserDTO).Id;
            var disId = uOW.LocalizationRepo.Get().Where(d => d.UserId == id).First().DistrictId;
            var disname = uOW.DistrictRepo.GetByID(disId).Name;
            ViewBag.DistrictName = disname;
            var item = localmanager.GetById(id);
            if (item != null)
            {
                return View();
            }
            return RedirectToAction("CreateLocation");
        }

        [HttpPost]
        public ActionResult EditLocation(string Name)
        {
            int districtId = uOW.DistrictRepo.Get().Where(x => x.Name == Name).First().Id;
            int localid = uOW.LocalizationRepo.Get().Where(x => x.DistrictId == districtId).First().LocalizationId;
            LocalizationDTO district = new LocalizationDTO()
            {
                LocalizationId = localid,
                UserId = (Session["User"] as Model.DTO.UserDTO).Id,
                DistrictId = districtId
            };
            localmanager.UpdateLocalization(district);
            return RedirectToAction("Index", "Home"); ;
        }

        [HttpGet]
        public ActionResult CreateLocation()
        {
            var listDistricts = uOW.DistrictRepo.Get().ToList();
            ViewBag.Districts = listDistricts;
            return View();
        }
        [HttpPost]
        public ActionResult CreateLocation(string Name)
        {
            int districtId = uOW.DistrictRepo.Get().Where(x => x.Name == Name).First().Id;
            LocalizationDTO district = new LocalizationDTO()
            {

                UserId = (Session["User"] as Model.DTO.UserDTO).Id,
                DistrictId = districtId
            };
            { }
            localmanager.AddLoc(district);
            return RedirectToAction("Index", "Home");
        }
    }
}
