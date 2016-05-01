using BAL.Interfaces;
using Microsoft.AspNet.SignalR;
using Model.DTO;
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
		private IDriverExManager driverManager;

		public DriverExController(IOrderManagerEx orderManager, IDriverExManager driverManager)
		{
			this.orderManager = orderManager;
			this.driverManager = driverManager;
		}

		public ActionResult Index()
		{
			return View(orderManager.GetApprovedOrders());
		}

		[HttpPost]
		public JsonResult TakeOrder(int id, int WaitingTime)
		{
			orderManager.SetWaitingTime(id, WaitingTime);
			return Json(new { success = orderManager.TakeOrder(id) });
		}

		[HttpPost]
		public void SetCoordinate(CoordinatesExDTO coordinate)
		{
			coordinate.DriverId = (Session["User"] as UserDTO).Id;
			driverManager.AddDriverLocation(coordinate);
		}
	}
}
