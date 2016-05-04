using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class TariffExController : Controller
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

    }
}
