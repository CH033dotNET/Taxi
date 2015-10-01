using MainSaite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class TarifController : BaseController
    {
        //
        // GET: /Tarif/
      

        public ActionResult Index()
        {
            var lst = tarifManager.GetTarifes().ToList();
            return View(lst);
        }

        public ActionResult Create()
        {
            CreateTarifModel model = new CreateTarifModel();

            model.Districts = tarifManager.getDistrictsList();

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(TarifDTO tarif)
        {
            tarifManager.AddTarif(tarif);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0)
        {
            CreateTarifModel model = new CreateTarifModel();
            model.Tarif = tarifManager.GetById(id);
            model.Districts = tarifManager.getDistrictsList();

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(TarifDTO tarif)
        {
            tarifManager.UpdateTarif(tarif);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            tarifManager.DeleteTarif(id);
            return RedirectToAction("Index");
        }



    }
}
