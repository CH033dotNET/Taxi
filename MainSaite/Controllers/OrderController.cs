using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DB;
using BAL.Manager;

namespace MainSaite.Controllers
{
    public class OrderController : BaseController
    {
		private IOrderManager orderManager;

		public OrderController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}

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

		public ActionResult InsertData(string UserId, string ComeOutlatitude, string ComeOutlongitude, string ComeInlatitude,string ComeInlongitude)
		{

			//var order = new Order() { LatitudePeekPlace = float.Parse(ComeInlatitude), 
			//	LongitudePeekPlace = float.Parse(ComeInlongitude), 
			//	LatitudeDropPlace = float.Parse(ComeOutlatitude), 
			//	LongitudeDropPlace = float.Parse(ComeOutlongitude),
			//};
			////orderManager.InsertOrder();
		    return new EmptyResult();
	    }
        public ActionResult JsonOrdersData() 
        {
            var data = orderManager.GetOrders();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}

