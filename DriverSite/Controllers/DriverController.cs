using Common.Enum;
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
using DriverSite.Helpers;

namespace DriverSite.Controllers
{
    public class DriverController :BaseController
    {
        readonly string controller = "Driver";
        public ActionResult DistrictPart()
		{
            return PartialView(ApiRequestHelper.GetById<bool>(controller, "GetWorkShiftsByWorkerId", SessionUser.Id).Data);            //return PartialView(carManager.GetWorkingDrivers());
		}

		public JsonResult GetDistricts()
		{
			return Json(ApiRequestHelper.GetById<List<DriverDistrictInfoDTO>>(controller, "GetDriverDistrictInfo", SessionUser.Id).Data, JsonRequestBehavior.AllowGet);
		}


		public JsonResult WorkStateChange(int Id, string Latitude, string Longitude, string Accuracy, string TimeStart)
		{
			try
			{
				if (Latitude != null && Longitude != null)
				{
					CoordinatesDTO coordinates;
                    coordinates = CoordinateMapper.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					//coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
                    coordinates.TarifId = 1;
                    ApiRequestHelper.postData<CoordinatesDTO>(controller, "AddCoordinates", coordinates);
					//coordinatesManager.AddCoordinates(coordinates);
				}

				ApiRequestHelper.postData<DriverLocation>(controller, "StartWorkEvent", new DriverLocation() {
					id = Id,
					latitude = double.Parse(Latitude, CultureInfo.InvariantCulture), 
					longitude = double.Parse(Longitude, CultureInfo.InvariantCulture), 
					startedTime = DateTime.Now, 
					updateTime = DateTime.Now,
					name = SessionUser.UserName
				});
				//carManager.StartWorkEvent(Id, TimeStart);
                //-Hub PROBLEM!!!--------------------------driverLocationHelper.addDriver();
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
                    coordinates = CoordinateMapper.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
					//coordinates = coordinatesManager.InitializeCoordinates(Longitude, Latitude, Accuracy, Id);
                    coordinates.TarifId = 1;
                    ApiRequestHelper.postData<CoordinatesDTO>(controller, "AddCoordinates", coordinates);
					//coordinatesManager.AddCoordinates(coordinates);
				}
                ApiRequestHelper.Get<object, int, string>(controller, "EndAllCurrentUserShifts", Id, TimeStop);
				//carManager.EndAllCurrentUserShifts(Id, TimeStop);

                if (ApiRequestHelper.GetById<LocationDTO>(controller, "GetByUserId", Id).Data != null)
                //if (locationManager.GetByUserId(Id) != null)
                    ApiRequestHelper.GetById<LocationDTO>(controller, "DeleteLocation", Id);
                    //locationManager.DeleteLocation(Id);
                //-Hub PROBLEM!!!-------------------------------driverLocationHelper.removeDriver(Id);
				return Json(true);
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return Json(false);
		}
		/// <summary>
		/// Action method used to add location info (district) for current driver. If user is unauthorized it redirects it to home page.
		/// If user is authorized it checks db for existing location for this user. If search is successfull, it updates it.
		/// If not - creates new.
		/// </summary>
		/// <param name="Id">District ID</param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult JoinToLocation(int Id)
		{
            LocationDTO local = ApiRequestHelper.GetById<LocationDTO>(controller, "GetByUserId", SessionUser.Id).Data;
			if (local != null)
			{
				local.DistrictId = Id;
                ApiRequestHelper.postData<LocationDTO>(controller, "UpdateLocation", local);				//locationManager.UpdateLocation(local);
				return Json(0);
			}

			else
			{
				LocationDTO district = new LocationDTO()
				{
					UserId = SessionUser.Id,
					DistrictId = Id
				};

                ApiRequestHelper.postData<LocationDTO>(controller, "AddLocation", district);
				//locationManager.AddLocation(district);
				return Json(0);
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

            var orders = ApiRequestHelper.Get<List<OrderDTO>>(controller, "GetDriverOrders").Data;
            //var orders = orderManager.GetDriverOrders();
			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public void GetOrder(int orderId, string waitingTime)
        {
            var order = ApiRequestHelper.GetById<OrderDTO>(controller, "GetOrderByOrderID", orderId).Data;
            //var order = orderManager.GetOrderByOrderID(orderId);
			order.DriverId = SessionUser.Id;
			order.WaitingTime = waitingTime;
            ApiRequestHelper.postData<OrderDTO>(controller, "EditOrder", order);
			//orderManager.EditOrder(order);
		}
	}
}
