using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class DriveClientHistoryController : BaseController
    {
		
        public ActionResult ShowHistory()
        {
			var clientDriveHistory = orderManager.GetOrders();
			return View(clientDriveHistory);
        }
    }
}
