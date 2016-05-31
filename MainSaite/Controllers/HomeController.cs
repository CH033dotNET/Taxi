using BAL.Interfaces;
using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class HomeController : BaseController
	{
		private INewsManager newsManager;
		private IPersonManager personManager;

		public HomeController(INewsManager newsManager, IPersonManager personManager)
		{
			this.newsManager = newsManager;
			this.personManager = personManager;
		}

		public ActionResult Index()
		{
			if(Session["Culture"]==null)
			   Session["Culture"] = "en-us";

			ViewBag.News = newsManager.GetLatestNews(4);
			return View();
		}


		public ActionResult SetLanguage(string language, string returnUrl)
		{
			Session["Culture"] = language;
            if (SessionUser != null)
                SessionUser.Lang = language;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			return Redirect(returnUrl);
		}

		public ActionResult OrderTaxi()
		{
			return View("OrderTaxi");
		}

		public ActionResult OrderForm()
		{
			return View();
		}

		public JsonResult GetBestDrivers()
		{
			return Json(personManager.GetBestPersons(AvailableRoles.Driver, 6));
		}

		public JsonResult GetBestClients()
		{
			return Json(personManager.GetBestPersons(AvailableRoles.Client, 6));
		}
	}
}
