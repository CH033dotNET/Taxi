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
            if (Session["User"] != null)
            {
                return RedirectToAction("UsersMenu", "Settings");
            }
			ViewBag.Hello = Resources.Resource.Hello;
            return View();
        }

		[HttpPost]
		public ActionResult Index(string language)
		{
			Session["Culture"] = language;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			ViewBag.Hello = Resources.Resource.Hello;
			return View();
		}



    }
}
