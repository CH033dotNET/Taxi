﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using Model.DTO;
//using MainSaite.Helpers;

namespace TaxiAPI.Controllers
{
    public class DriverController : BaseController
    {
		private ILocationManager locationManager;
		private ICarManager carManager;
		private ICoordinatesManager coordinatesManager;
		private IUserManager userManager;
        private IOrderManager orderManager;
        public DriverController(ILocationManager locationManager, ICarManager carManager, ICoordinatesManager coordinatesManager, IUserManager userManager, IOrderManager orderManager)
		{
			this.locationManager = locationManager;
			this.carManager = carManager;
			this.coordinatesManager = coordinatesManager;
			this.userManager = userManager;
			this.coordinatesManager.addedCoords += coordinates => MainSiteRequestHelper.postData<bool,CoordinatesDTO>("OperatorHub", "AddedLocation", coordinates);
        }
        [HttpGet]
        //[Route("api/Driver/GetDriverDistrictInfo")]
        public HttpResponseMessage GetDriverDistrictInfo(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.GetDriverDistrictInfo(id));
        }
        [HttpGet]
        //[Route("api/Driver/getDistricts")]
        public HttpResponseMessage GetWorkingDrivers()
        {
            return Request.CreateResponse(HttpStatusCode.OK, carManager.GetWorkingDrivers());
        }
        [HttpGet]
        //[Route("api/Driver/GetWorkShiftsByWorkerId")]
        public HttpResponseMessage GetWorkShiftsByWorkerId(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, carManager.GetWorkShiftsByWorkerId(id));
        }
        [HttpPost]
        //[Route("api/Driver/AddCoordinates")]
        public HttpResponseMessage AddCoordinates(CoordinatesDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, coordinatesManager.AddCoordinates(data));
        }
        [HttpPost]
        //[Route("api/Driver/StartWorkEvent")]
        public HttpResponseMessage StartWorkEvent(DriverLocation data)
        {
            carManager.StartWorkEvent(data.id, data.startedTime.ToString());
			MainSiteRequestHelper.postData<bool, DriverLocation>("OperatorHub", "AddDriver", data);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        //[Route("api/Driver/EndAllCurrentUserShifts")]
        public HttpResponseMessage EndAllCurrentUserShifts(int param1, string param2)
        {
            carManager.EndAllCurrentUserShifts(param1, param2);
			MainSiteRequestHelper.GetById<bool>("OperatorHub", "RemoveDriver", param1);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        //[Route("api/Driver/GetByUserId")]
        public HttpResponseMessage GetByUserId(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.GetByUserId(id));
        }
        [HttpGet]
        //[Route("api/Driver/DeleteLocation")]
        public HttpResponseMessage DeleteLocation(int id)
        {
            locationManager.DeleteLocation(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPost]
        //[Route("api/Driver/UpdateLocation")]
        public HttpResponseMessage UpdateLocation(LocationDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.UpdateLocation(data));
        }
        [HttpPost]
        //[Route("api/Driver/AddLocation")]
        public HttpResponseMessage AddLocation(LocationDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.AddLocation(data));
        }
        [HttpGet]
        //[Route("api/Driver/GetDriverOrders")]
        public HttpResponseMessage GetDriverOrders()
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetDriverOrders());
        }
        [HttpGet]
        //[Route("api/Driver/GetOrderByOrderID")]
        public HttpResponseMessage GetOrderByOrderID(int? id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetOrderByOrderID(id));
        }
        [HttpPost]
        //[Route("api/Driver/EditOrder")]
        public HttpResponseMessage EditOrder(OrderDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.EditOrder(data));
        }
    }
}
