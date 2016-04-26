using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;

namespace MainSaite.Controllers
{
	public class OrderExController : Controller
	{
		//
		// GET: /OrderEx/

		public ActionResult Index()
		{
			return View(new List<OrderExDTO>());
		}

	}
}
