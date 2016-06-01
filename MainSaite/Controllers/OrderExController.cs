using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using BAL.Interfaces;
using Microsoft.AspNet.SignalR;
using MainSaite.Hubs;
using BAL.Manager;
using MainSaite.Helpers;

namespace MainSaite.Controllers
{
	public class OrderExController : BaseController
	{

		private IOrderManagerEx orderManager;
		private IDistrictManager districtManager;

		public OrderExController(IOrderManagerEx orderManager, IDistrictManager districtManager)
		{
			this.orderManager = orderManager;
			this.districtManager = districtManager;
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		public ActionResult Index()
		{
			return View();
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult ApproveOrder(int id)
		{
			return Json(new { success = orderManager.ApproveOrder(id) });
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult DenyOrder(int id)
		{
			return Json(new { success = orderManager.DenyOrder(id) });
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult GetOrder(int id)
		{
			return Json(new { success = orderManager.GetById(id) });
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult GetOperatorOrders()
		{
			return Json(new
			{
				NewOrders = orderManager.GetNotApprovedOrders(),
				ApprovedOrders = orderManager.GetApprovedOrders(),
				DeniedOrders = orderManager.GetLastDeniedOrders(),
				InProgressOrders = orderManager.GetInProgressOrders(),
				FinishedOrders = orderManager.GetFinishedOrders()
			});
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		public PartialViewResult GetOrderById(int id = 0)
		{
			var order = orderManager.GetById(id);
			return PartialView("_EditOrder", order);
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult EditOrder(OrderExDTO order)
		{
			orderManager.UpdateOrder(order);
			return Json(true);
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult FinishOrder(decimal price, int id = 0)
		{
			bool success = orderManager.FinishOrder(id, price);
			return Json(true);
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult GetOrderAddressByID(int id = 0)
		{
			string address = orderManager.GetById(id).FullAddressFrom;
			return Json(new{ address });
		}

		[AuthFilter(Roles = "Operator, Administrator")]
		[HttpPost]
		public JsonResult GetDistricts()
		{
			return Json(new { districts = districtManager.GetFilesDistricts() });
		}

	}
}
