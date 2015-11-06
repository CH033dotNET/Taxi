using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using Model.DTO;
using DAL.Interface; //????

namespace TaxiAPI.Controllers
{
	public class LocationController : ApiController
	{
		private ILocationManager locationManager;
		private IDistrictManager districtManager;

		public LocationController(ILocationManager locationManager, IDistrictManager districtManager)
		{
			this.locationManager = locationManager;
			this.districtManager = districtManager;
		}
        [HttpGet]
        [Route("api/Location/getDistricts")]
        public HttpResponseMessage getDistricts()
        {
            return Request.CreateResponse(HttpStatusCode.OK, districtManager.getDistricts());
        }
        [HttpGet]
        [Route("api/Location/GetByUserId")]
        public HttpResponseMessage GetByUserId(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.GetByUserId(id));
        }
        [HttpGet]
        [Route("api/Location/getById")]
        public HttpResponseMessage getById(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, districtManager.getById(id));
        }
        [HttpPost]
        [Route("api/Location/UpdateLocation")]
        public HttpResponseMessage UpdateLocation(LocationDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.UpdateLocation(data));
        }
        [HttpPost]
        [Route("api/Location/AddLocation")]
        public HttpResponseMessage AddLocation(LocationDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, locationManager.AddLocation(data));
        }
	}
}
