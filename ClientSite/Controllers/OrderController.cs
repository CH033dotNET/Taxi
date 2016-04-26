using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DB;
using Model.DTO;
using Model;
using ClientSite.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using ClientSite.Helpers;
using System.Net;
namespace ClientSite.Controllers
{
	public class OrderController : BaseController
	{
		//Client site
		public ActionResult Index()
		{

			return View();
		}

		public JsonResult NewOrder(OrderDTO order)
		{
     		order.PersonId = SessionPerson.Id;

			order = ApiRequestHelper.postData<OrderDTO>("Order", "NewOrder", order).Data;

			return Json(order, JsonRequestBehavior.AllowGet);
		}
		public JsonResult SendNewOrderToOperators(OrderDTO order)
		{
			ApiRequestHelper.postData<OrderDTO>("Orders", "SendNewOrderToOperators", order);
			return Json(order, JsonRequestBehavior.AllowGet);
		}

		//public JsonResult WhereMyDriver(int orderId)
		//{
		//	var result = coordinatesManager.GetCoordinatesByOrdeId(orderId);
		//	return Json(result, JsonRequestBehavior.AllowGet);
		//}
	}
}