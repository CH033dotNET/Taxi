using Common.Enum;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DriverSite.Controllers
{
	public class OrdersController : BaseController
	{
		string controller = "Orders";
		public ActionResult Index()
		{
			return PartialView();
		}
		public JsonResult GetDriverOrders()
		{
			var orders = ApiRequestHelper.Get<List<OrderDTO>>(controller, "GetOrders").Data;
			//var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0).ToList();
			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCurrentOrder(int orderId)
		{
			var currentOrder = ApiRequestHelper.GetById<OrderDTO>(controller, "GetCurrentOrder", orderId).Data;
			//var currentOrder = orderManager.GetOrderByOrderID(orderId);
			return Json(currentOrder, JsonRequestBehavior.AllowGet);
		}

		public JsonResult SendToOperators(string message, string userName)
		{
			var currentOrder = ApiRequestHelper.Get<bool, string, string>(controller, "SendToOperators", message, userName).Data;
			//var currentOrder = orderManager.GetOrderByOrderID(orderId);
			return Json(currentOrder, JsonRequestBehavior.AllowGet);
		}

		public void AssignCurrentOrder(OrderDTO currentOrder)
		{
			var current = ApiRequestHelper.postData<OrderDTO>(controller, "AssignCurrentOrder", currentOrder).Data;
			//var currentOrder = orderManager.GetOrderByOrderID(orderId);
		}
	}
}
