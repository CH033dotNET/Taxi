using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
		protected UnitOfWork uOW;

		public BaseController()
		{
			uOW = new UnitOfWork();
		}

        public ActionResult Index()
        {
            return View();
        }

    }
}
