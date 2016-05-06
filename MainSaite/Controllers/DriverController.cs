using BAL.Interfaces;
using BAL.Manager;
using Common.Enum;
using DAL;
using MainSaite.Helpers;
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
		private IWorkerStatusManager workerStatusManager;
		private IDistrictManager districtManager;
		public DriverController(ILocationManager locationManager, ICarManager carManager, ICoordinatesManager coordinatesManager, IUserManager userManager, IDriverLocationHelper driverLocationHelper, IOrderManager orderManager,ITarifManager tarifManager, IWorkerStatusManager workerStatusManager, IDistrictManager districtManager) 
		{
			this.locationManager = locationManager;
			this.carManager = carManager;
			this.coordinatesManager = coordinatesManager;
			this.userManager = userManager;
			this.driverLocationHelper = driverLocationHelper;
			this.coordinatesManager.addedCoords += coordinates => DriverLocationHelper.addedLocation(coordinates);
			this.orderManager = orderManager;
            this.tarifManager = tarifManager;
			this.workerStatusManager = workerStatusManager;
			this.districtManager = districtManager;
		}

		public ActionResult Index()
		{
			if (SessionUser != null && (SessionUser.RoleId == (int)AvailableRoles.Driver || SessionUser.RoleId == (int)AvailableRoles.FreeDriver))
			{
				return View();
			}
			else
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

		public ActionResult DistrictPart()
		{
			return PartialView(carManager.GetWorkShiftsByWorkerId(SessionUser.Id));            //return PartialView(carManager.GetWorkingDrivers());
		}
		/// <summary>
		/// Action method used to add location info (district) for current driver. If user is unauthorized it redirects it to home page.
		/// If user is authorized it checks db for existing location for this user. If search is successfull, it updates it.
		/// If not - creates new.
		/// </summary>
		/// <param name="Id">District ID</param>
		/// <returns></returns>
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
			var orders = orderManager.GetOrders().Where(x => x.IsConfirm == 1 && x.DriverId == 0).ToList();
			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCurrentOrder(int orderId)
		{
			var currentOrder = orderManager.GetOrderByOrderID(orderId);
			return Json(currentOrder, JsonRequestBehavior.AllowGet);
		}


		public JsonResult GetDistricts()
		{
			return Json(locationManager.GetDriverDistrictInfo(SessionUser.Id), JsonRequestBehavior.AllowGet);
		}

		public JsonResult CheckDriverMainCar()
		{
			var result = carManager.FindMainCar(SessionUser.Id);
			if (!result) { return Json(new { success = false }, JsonRequestBehavior.AllowGet); }
			else
			{
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
		}
		public JsonResult GetCurrentDriverStatus()
		{
			var result = workerStatusManager.ShowStatus(SessionUser.Id);

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
				workerStatusManager.ChangeWorkerStatus(SessionUser.Id, status.ToString());
			}
			catch (Exception)
			{
				return Json( false, JsonRequestBehavior.AllowGet);
			}

				return Json(true, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult GetFullDistricts()
		{
			var districts = districtManager.getDistricts();
			return Json(new { districts = districts}, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult JoinDriverToLocation(int Id)
		{
			LocationDTO local = locationManager.GetByUserId(Id);
			if (local != null)
			{
				local.DistrictId = Id;
				locationManager.UpdateLocation(local);                //locationManager.UpdateLocation(local);
				return Json(0);
			}

			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = SessionUser.Id,
					DistrictId = Id
				};

				locationManager.AddLocation(district);
				return Json(0);
			}
		}
		public JsonResult WorkStateChange(int Id, string Latitude, string Longitude, string Accuracy, string TimeStart)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					DriverLocation driverLocation = new DriverLocation()
					{
						id = Id,
						latitude = double.Parse(Latitude, CultureInfo.InvariantCulture),
						longitude = double.Parse(Longitude, CultureInfo.InvariantCulture),
						startedTime = DateTime.Now,
						updateTime = DateTime.Now,
						name = SessionUser.UserName
					};

					DriverLocationHelper.addDriver(driverLocation);

					CoordinatesDTO coordinates;
					coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					coordinates.TarifId = 1;
					coordinatesManager.AddCoordinates(coordinates);
				}

				carManager.StartWorkEvent(Id, DateTime.Now.ToString());


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
					DriverLocationHelper.removeDriver(Id);

					CoordinatesDTO coordinates;
					//coordinates = CoordinateMapper.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					//coordinates.TarifId = 1;
					//coordinatesManager.AddCoordinates(coordinates);
				}
				carManager.EndAllCurrentUserShifts(Id, TimeStop);


				if (locationManager.GetByUserId(Id)!= null)
					locationManager.DeleteLocation(Id);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
	}
}
