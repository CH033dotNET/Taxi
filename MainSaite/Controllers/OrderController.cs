using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

		//[HttpPost]
		//public ActionResult Index()
		//{
		//	// id - имя клиента, заказы которого необходимо выводить на странице.
		//	return View("Index", (object)id);
		//}

        public ActionResult OrdersData()
        {
	        var data = orderManager.GetOrders();
           
            return PartialView(data);
        }

        public ActionResult JsonOrdersData() 
        {
            var data = orderManager.GetOrders();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}

