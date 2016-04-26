using Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			Session["Culture"] = "en-us";
			return View();
		}

		public ActionResult SetLanguage(string language, string returnUrl)
		{
			Session["Culture"] = language;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			return Redirect(returnUrl);
		}
	}
}