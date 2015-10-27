using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class DrivingSimulationController : BaseController
	{


		public ActionResult Index()
		{
			return PartialView();
		}

	}
}
