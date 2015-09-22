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
		//
		// GET: /Home/
		
		public ActionResult Index()
		{
			return View();
		}


		public ActionResult SetLanguage(string language, string returnUrl)
		{
			Session["Culture"] = language;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			ViewBag.Hello = Resources.Resource.Hello;
			return Redirect(returnUrl);
		}



	}
}
