using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading;

namespace DriverSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (SessionUser == null || SessionUser.UserName == null)
            {
                return RedirectToAction("Authentification", "Account");
            }
            else
            {
                return View();
            }
        }
    }
}