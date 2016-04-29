using BAL.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class DriverExController : Controller
    {
        private IOrderManagerEx orderManager;

        public DriverExController(IOrderManagerEx orderManager) {
            this.orderManager = orderManager;
        }


        //
        // GET: /DriverEx/

        public ActionResult Index() {
            return View(orderManager.GetApprovedOrders());
        }

        [HttpPost]
        public JsonResult TakeOrder(int id) {
            return Json(new { success = orderManager.TakeOrder(id) });
        }
    }
}
