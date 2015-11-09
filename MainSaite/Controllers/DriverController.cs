using BAL.Manager;
using Common.Enum;
using DAL;
using MainSaite.Helpers;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MainSaite.Controllers
{
	public class DriverController : BaseController
	{
		private ILocationManager locationManager;
		private ICarManager carManager;
		private ICoordinatesManager coordinatesManager;
		private IUserManager userManager;
		private IDriverLocationHelper driverLocationHelper;
		private IOrderManager orderManager;
        private ITarifManager tarifManager;
		public DriverController(ILocationManager locationManager, ICarManager carManager, ICoordinatesManager coordinatesManager, IUserManager userManager, IDriverLocationHelper driverLocationHelper, IOrderManager orderManager,ITarifManager tarifManager) 
		{
			this.locationManager = locationManager;
			this.carManager = carManager;
			this.coordinatesManager = coordinatesManager;
			this.userManager = userManager;
			this.driverLocationHelper = driverLocationHelper;
			this.coordinatesManager.addedCoords += this.driverLocationHelper.addedLocation;
			this.orderManager = orderManager;
            this.tarifManager = tarifManager;
		}

		public ActionResult Index()
		{
			if (null == Session["User"] || ((UserDTO)Session["User"]).RoleId != (int)AvailableRoles.Driver)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			else
			{
				return View();
			}
		}

		public ActionResult DistrictPart()
		{
			ViewBag.Districts = locationManager.GetDriverDistrictInfo();
            Tarifes = tarifManager.GetTarifes().ToList();
			return PartialView(carManager.GetWorkingDrivers());
		}
		/// <summary>
		/// Checks if there are uncompleted workshifts for current Driver
		/// </summary>
		/// <param name="Id">drivers id</param>
		/// <returns></returns>
		public JsonResult CheckWorkShifts(int Id)
		{
			bool uncompletedShifts = carManager.GetWorkShiftsByWorkerId(Id);
            Tarifes = tarifManager.GetTarifes().ToList();
			return Json(uncompletedShifts, JsonRequestBehavior.AllowGet);
		}


		public JsonResult WorkStateChange(int Id, string Latitude, string Longitude, string Accuracy, string TimeStart)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					CoordinatesDTO coordinates;
					coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					coordinates.TarifId = 1;
					coordinatesManager.AddCoordinates(coordinates);
				}
				carManager.StartWorkEvent(Id, TimeStart);
                driverLocationHelper.addDriver(Id, double.Parse(Latitude, CultureInfo.InvariantCulture), double.Parse(Longitude, CultureInfo.InvariantCulture), DateTime.Now, userManager.GetById(Id).UserName);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		public JsonResult WorkStateEnded(int Id, string Latitude, string Longitude, string Accuracy, string TimeStop)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					CoordinatesDTO coordinates;
					coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					coordinates.TarifId = 1;
					coordinatesManager.AddCoordinates(coordinates);
				}
				carManager.EndAllCurrentUserShifts(Id, TimeStop);
				//carManager.EndWorkShiftEvent(user.Id);
				if (locationManager.GetByUserId(Id) != null)
                    locationManager.DeleteLocation(Id);
                driverLocationHelper.removeDriver(Id);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		public ActionResult JoinToLocation(int Id)
		{
			var user = Session["User"] as Model.DTO.UserDTO;
			if (user == null)
			{
				return RedirectToRoute(new
				{
					controller = "Home",
					action = "Index"
				});
			}
			LocationDTO local = locationManager.GetByUserId(user.Id);
			if (local != null)
			{
				local.DistrictId = Id;
				locationManager.UpdateLocation(local);
				return RedirectToAction("Index", "Driver");
			}

			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = user.Id,
					DistrictId = Id
				};
				locationManager.AddLocation(district);
				return RedirectToAction("Index", "Driver");
			}
		}
		public ActionResult GetArticle(string timeStart, string timeStop)
		{

			string format = "d HH:mm:ss tt";
			try
			{

				DateTime dt = DateTime.ParseExact(timeStart, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

				string windowsTime = "2/21/2009 10:35 PM";

				var time = DateTime.Parse(windowsTime);
				Console.WriteLine(time);
				DateTime result = DateTime.Parse(timeStart);
			}
			catch (FormatException)
			{
				Console.WriteLine("{0} is not in the correct format.", timeStart);

			}

			var dateString = "15/06/2008 08:30";
			format = "g";
			var provider = new CultureInfo("fr-FR");
			try
			{
				var result = DateTime.ParseExact(dateString, format, provider);
				Console.WriteLine("{0} converts to {1}.", dateString, result.ToString());
			}
			catch (FormatException)
			{
				Console.WriteLine("{0} is not in the correct format.", dateString);
			}
			return new EmptyResult();
		}


		public ActionResult DriverOrders()
		{
			return PartialView();
		}

		public JsonResult GetDriverOrders()
		{
			var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0);
			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public void GetOrder(int orderId, string waitingTime)
		{
			var order = orderManager.GetOrderByOrderID(orderId);
			order.DriverId = SessionUser.Id;
			order.WaitingTime = waitingTime;
			orderManager.EditOrder(order);
		}


	}
}
