using BAL.Manager;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class DriveClientHistoryController : BaseController
    {
		private IOrderManager orderManager;
		public DriveClientHistoryController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}

        public ActionResult ShowHistory()
        {
			return View();
        }

		public JsonResult GetHistory()
		{ 
			var clientDriveHistory = orderManager.GetOrdersByPersonId(SessionUser.Id);
			return Json(clientDriveHistory, JsonRequestBehavior.AllowGet);
		}
    }
}
