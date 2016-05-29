using BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class NewsController : BaseController
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

		public bool DeleteArticle(int id)
		{
			return NewsManager.DeleteArticle(id);
		}

		public ActionResult Edit(int id)
		{
			return View(NewsManager.GetOneArticle(id));
		}
	}
}
