using BAL.Interfaces;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class TariffExController : BaseController
	{
		private ITariffExManager TariffExManager;

		public TariffExController(ITariffExManager manager)
		{
			TariffExManager = manager;
		}

		//
		// GET: /TariffEx/

		public ActionResult Index()
		{
			return View(TariffExManager.GetAllTariffs());
		}

		public JsonResult GetAllActiveTariffs()
		{
			return Json(TariffExManager.GetAllTariffs().Where(e => e.Status == Common.Enum.TariffExStatus.Active).ToList(), JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetTariffData(int id)
		{
			return Json(TariffExManager.GetTariffData(id), JsonRequestBehavior.AllowGet);
		}

		public bool SaveTariff(TariffExDTO tariff)
		{
			return TariffExManager.SaveTariff(tariff);
		}

		public bool DeleteTariff(int id)
		{
			return TariffExManager.DeleteTariff(id);
		}

	}
}
