using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using BAL.Interfaces;
using Microsoft.AspNet.SignalR;
using MainSaite.Hubs;

namespace MainSaite.Controllers
{
	public class OrderExController : BaseController
	{

		private IOrderManagerEx orderManager;

		private static IHubContext Context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();

		public OrderExController(IOrderManagerEx orderManager)
		{
			this.orderManager = orderManager;
		}

		public ActionResult Index()
		{
			return View(orderManager.GetNotApprovedOrders());
		}

		[HttpPost]
		public JsonResult ApproveOrder(int id)
		{
			return Json( new { success = orderManager.ApproveOrder(id) });
		}

		[HttpPost]
		public JsonResult DenyOrder(int id)
		{
			return Json(new { success = orderManager.DenyOrder(id) });
		}

	}
}
