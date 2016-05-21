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

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult ApproveOrder(int id)
		{
			return Json(new { success = orderManager.ApproveOrder(id) });
		}

		[HttpPost]
		public JsonResult DenyOrder(int id)
		{
			return Json(new { success = orderManager.DenyOrder(id) });
		}

		[HttpPost]
		public JsonResult GetOrder(int id)
		{
			return Json(new { success = orderManager.GetById(id) });
		}

		[HttpPost]
		public JsonResult GetOperatorOrders()
		{
			return Json(new
			{
				NewOrders = orderManager.GetNotApprovedOrders(),
				ApprovedOrders = orderManager.GetApprovedOrders(),
				DeniedOrders = orderManager.GetLastDeniedOrders(),
				InProgressOrders = orderManager.GetInProgressOrders()
			});
		}

		[HttpPost]
		public JsonResult GetDistricts()
		{
			return Json(new { districts = districtManager.GetFilesDistricts() });
		}

	}
}
