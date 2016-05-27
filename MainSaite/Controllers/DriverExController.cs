using BAL.Interfaces;
using BAL.Manager;
using MainSaite.Helpers;
using Microsoft.AspNet.SignalR;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common.Enum.DriverEnum;
using Common.Enum;

namespace MainSaite.Controllers
{
	public class DriverExController : BaseController
	{
		private IOrderManagerEx orderManager;
		private IDriverExManager driverManager;
		private IDistrictManager districtManager;
		private IFeedbackManager feedbackManager;
		private ICarManager carManager;
		private IWorkerStatusManager workerStatusManager;
		private IUserManager userManager;

		public DriverExController(
			IFeedbackManager feedbackManager,
			IOrderManagerEx orderManager,
			IDriverExManager driverManager,
			IDistrictManager districtManager,
			ICarManager carManager,
			IWorkerStatusManager workerStatusManager,
			IUserManager userManager)
		{
			this.orderManager = orderManager;
			this.driverManager = driverManager;
			this.districtManager = districtManager;
			this.feedbackManager = feedbackManager;
			this.carManager = carManager;
			this.workerStatusManager = workerStatusManager;
			this.userManager = userManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult NewOrders()
		{
			return PartialView(orderManager.GetApprovedOrders());
		}

		public ActionResult Districts()
		{
			return PartialView(districtManager.GetFilesDistricts());
		}

		public ActionResult OrdersHistory()
		{
			var driver = (Session["User"] as UserDTO);
			return PartialView(orderManager.GetOrdersByDriver(driver));
		}

		public ActionResult MyOrder()
		{
			var driverOrder = orderManager.GetCurrentDriverOrder((Session["User"] as UserDTO).Id);
			if (driverOrder != null)
			{
				if (driverOrder.AdditionallyRequirements == null)
				{
					// We should show additional requirements always!
					// If user didn't choose them than we should use default settings and driver can change them.
					driverOrder.AdditionallyRequirements = new AdditionallyRequirementsDTO();
					driverOrder.AdditionallyRequirements.Urgently = true;
				}
			}
			return PartialView(driverOrder);
		}

		public ActionResult Pulse()
		{
			return View();
		}

		[HttpPost]
		public JsonResult GetDriversWithsOrders(int id)
		{
			return Json(userManager.GetCurrentDrivers(id));
		}

		[HttpPost]
		public JsonResult GetDriversWithsOrdersLastMonth(int id)
		{
			return Json(userManager.GetCurrentDriversLastMonth(id));
		}

		[HttpPost]
		public JsonResult TakeOrder(int id, int WaitingTime)
		{
			var FREEDRIVER_TRIAL_DAYS = 15;
			var FREEDRIVER_ORDER_LIMIT = 5;

			var driver = (Session["User"] as UserDTO);
			var driverStatus = workerStatusManager.GetStatus(driver).WorkingStatus;

			// check freedriver trial period and today's order limit
			if (((Session["User"] as UserDTO).RoleId == (int)AvailableRoles.FreeDriver) &&
				((DateTime.Now - driver.RegistrationDate).Days > FREEDRIVER_TRIAL_DAYS ) &&
				(orderManager.GetDriversTodayOrders(driver).Count > FREEDRIVER_ORDER_LIMIT )) {

				Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return Json(new {
					errorHeader = Resources.Resource.ErrorHeader,
					errorMessage = Resources.Resource.FreeDriverOverlimitError });

			} else if (driverStatus == DriverWorkingStatusEnum.DoingOrder) {

				Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return Json(new {
					errorHeader = Resources.Resource.ErrorHeader,
					errorMessage = Resources.Resource.DriverHasOrderError
				});

			} else {

				workerStatusManager.ChangeStatus(driver, DriverWorkingStatusEnum.DoingOrder);
				orderManager.SetWaitingTime(id, WaitingTime);
				return Json(new { success = orderManager.TakeOrder(id, driver.Id) });

			}
		}

		[HttpPost]
		public void SetCoordinate(CoordinatesExDTO coordinate)
		{
			if (Session["User"] != null)
			{
				coordinate.DriverId = (Session["User"] as UserDTO).Id;
				driverManager.AddDriverLocation(coordinate);
				DriverLocationHelper.addedLocation(coordinate);
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
		public ActionResult WorkShift()
		{
			return PartialView(carManager.GetWorkShiftsByWorkerId((Session["User"] as UserDTO).Id));
		}
		public JsonResult CheckDriverMainCar()
		{
			var result = carManager.FindMainCar(((Session["User"] as UserDTO).Id));
			if (!result) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
			else
			{
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
		}
		public JsonResult GetCurrentDriverStatus()
		{
			var result = workerStatusManager.GetStatus((Session["User"] as UserDTO));

			if (result == null) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
			else
			{
				return Json(new { success = true, result.WorkingStatus }, JsonRequestBehavior.AllowGet);
			}
		}
		public JsonResult ChangeCurrentDriverStatus(int status)
		{
			try
			{
				workerStatusManager.ChangeStatus((Session["User"] as UserDTO), (DriverWorkingStatusEnum)status);
			}
			catch (Exception)
			{
				return Json(false, JsonRequestBehavior.AllowGet);
			}

			return Json(true, JsonRequestBehavior.AllowGet);
		}
		public JsonResult WorkShiftStarted(int Id, string Latitude, string Longitude, string Accuracy, string TimeStart)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					DriverLocationDTO driverLocation = new DriverLocationDTO()
					{
						id = Id,
						latitude = double.Parse(Latitude, CultureInfo.InvariantCulture),
						longitude = double.Parse(Longitude, CultureInfo.InvariantCulture),
						startedTime = DateTime.Now,
						updateTime = DateTime.Now,
						name = ((Session["User"] as UserDTO).UserName)
					};

					carManager.StartWorkEvent(Id, DateTime.Now.ToString());

					DriverLocationHelper.addDriver(driverLocation);

					CoordinatesExDTO coordinates = new CoordinatesExDTO
					{
						DriverId = Id,
						Latitude = double.Parse(Latitude, CultureInfo.InvariantCulture),
						Longitude = double.Parse(Longitude, CultureInfo.InvariantCulture),
						Accuracy = double.Parse(Accuracy, CultureInfo.InvariantCulture),
						AddedTime = DateTime.Parse(TimeStart)
					};

					driverManager.AddDriverLocation(coordinates);

				}

				return Json(true);
			}
			catch (Exception e)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}

		public JsonResult WorkShiftEnded(int Id, string Latitude, string Longitude, string Accuracy, string TimeStop)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					carManager.EndAllCurrentUserShifts(Id, TimeStop);

					DriverLocationHelper.removeDriver(Id);

					CoordinatesExDTO coordinates = new CoordinatesExDTO
					{
						DriverId = Id,
						Latitude = double.Parse(Latitude, CultureInfo.InvariantCulture),
						Longitude = double.Parse(Longitude, CultureInfo.InvariantCulture),
						Accuracy = double.Parse(Accuracy, CultureInfo.InvariantCulture),
						AddedTime = DateTime.Parse(TimeStop)
					};

					driverManager.AddDriverLocation(coordinates);
				}
				return Json(true);
			}
			catch (Exception e)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
	    [HttpGet]
		public JsonResult GetLoc()
		{			
			var array = driverManager.GetFullLocations();
			return Json(array, JsonRequestBehavior.AllowGet);
		}
	}
}
