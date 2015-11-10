using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class DriveClientHistoryController : BaseController
    {
        //
        // GET: /DriveClientHistory/
		public ActionResult ShowHistory()
		{
			return View();
		}

		public JsonResult GetHistory()
		{
			//var clientDriveHistory = orderManager.GetOrders();
			var clientDriveHistory = ApiRequestHelper.Get<List<OrderDTO>,int>("DriveClientHistory", "GetOrders", 1).Data as List<OrderDTO>;
			return Json(clientDriveHistory, JsonRequestBehavior.AllowGet);
		}
	}
}