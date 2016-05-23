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
		private IUserManager userManager;

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
			var FREEDRIVER_TRIAL_DAYS = 15;
			var FREEDRIVER_ORDER_LIMIT = 5;

			var DriverId = (Session["User"] as UserDTO).Id;
			var driver = userManager.GetById(driverId);

			// check freedriver trial period and today's order limit
			if (((DateTime.Now - driver.RegistrationDate).Days > FREEDRIVER_TRIAL_DAYS) &&
			//	(ApiRequestHelper.Get<List<OrderDTO>>("Orders", "GetTodayOrders").Data.Count > FREEDRIVER_ORDER_LIMIT)) {
			//	return Json("error", JsonRequestBehavior.AllowGet);
			//	// here must be message to driver
			//}

			orderManager.SetWaitingTime(id, WaitingTime);
			
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
