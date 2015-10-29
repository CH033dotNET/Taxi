using BAL.Manager;
using MainSaite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class TarifController : BaseController
    {
		private ITarifManager tarifManager;

		public TarifController(ITarifManager tarifManager)
		{
			this.tarifManager = tarifManager;
		}
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

        public JsonResult Edit(int id = 0)
        {
            CreateTarifModel model = new CreateTarifModel();
            model.Tarif = tarifManager.GetById(id);
            model.Districts = tarifManager.getDistrictsList();

            return Json(model,JsonRequestBehavior.AllowGet);
        }
		[HttpPost]
        public PartialViewResult Edit(TarifDTO tarif)
        {
            tarifManager.UpdateTarif(tarif);

			var model = tarifManager.GetTarifes();

			return PartialView("_ShowTarifTable", model);
        }
		[HttpPost]
        public PartialViewResult Delete(int id = 0)
        {
            tarifManager.DeleteTarif(id);

			var model = tarifManager.GetTarifes();

			return PartialView("_ShowTarifTable", model);
        }
    }
}
