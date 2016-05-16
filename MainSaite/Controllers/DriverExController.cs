using BAL.Interfaces;
using BAL.Manager;
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
		private IDistrictManager districtManager;
		private IFeedbackManager feedbackManager;

		public DriverExController(IFeedbackManager feedbackManager, IOrderManagerEx orderManager, IDriverExManager driverManager, IDistrictManager districtManager)
		{
			this.orderManager = orderManager;
			this.driverManager = driverManager;
			this.districtManager = districtManager;
			this.feedbackManager = feedbackManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult NewOrders() {
			return PartialView(orderManager.GetApprovedOrders());
		}

		public ActionResult Districts()
		{
			return PartialView(districtManager.GetFilesDistricts());
		}

		public ActionResult OrdersHistory() {
			var driver = (Session["User"] as UserDTO);
			return PartialView(orderManager.GetOrdersByDriver(driver));
		}

		public ActionResult MyOrder()
		{
			return PartialView();
		}

		[HttpPost]
		public JsonResult TakeOrder(int id, int WaitingTime)
		{
			orderManager.SetWaitingTime(id, WaitingTime);
			var DriverId = (Session["User"] as UserDTO).Id;
			return Json(new { success = orderManager.TakeOrder(id, DriverId) });
		}

		[HttpPost]
		public void SetCoordinate(CoordinatesExDTO coordinate)
		{
			if (Session["User"] != null)
			{
				coordinate.DriverId = (Session["User"] as UserDTO).Id;
				driverManager.AddDriverLocation(coordinate);
			}
		}

		[HttpPost]
		public JsonResult GetFeedback(int id)
		{
			return Json(feedbackManager.GetById(id));
		}

		[HttpPost]
		public JsonResult AddFeedback(FeedbackDTO feedback)
		{
			return Json(feedbackManager.AddFeedback(feedback));
		}

		[HttpPost]
		public JsonResult UpdateFeedback(FeedbackDTO feedback)
		{
			return Json(feedbackManager.UpdateFeedback(feedback));
		}

		[HttpPost]
		public void SetDriverFeedback(int orderId, int feedbackId)
		{
			orderManager.SetDriverFeedback(orderId, feedbackId);
		}
	}
}
