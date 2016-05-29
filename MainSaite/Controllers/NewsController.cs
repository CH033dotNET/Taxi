using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class NewsController : Controller
    {
		//
		// GET: /News/

		private INewsManager NewsManager;

		public NewsController(INewsManager newsManager)
		{
			NewsManager = newsManager;
		}
		public ActionResult Index()
        {
            return View(NewsManager.GetAllNews());
        }

    }
}
