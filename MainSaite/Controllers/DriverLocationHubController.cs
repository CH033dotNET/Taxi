using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using MainSaite.Helpers;

namespace MainSaite.Controllers
{
	public class DriverLocationHubController : ApiController
	{
		public DriverLocationHubController()
		{

		}
		//void addDriver(int Id, double Latitude, double Longitude, DateTime time, string username);
		[HttpPost]
		/*[Route("api/Driver/GetDriverDistrictInfo")]*/
		public HttpResponseMessage AddDriver(DriverLocation data)
		{
			DriverLocationHelper.addDriver(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
		[HttpGet]
		public HttpResponseMessage RemoveDriver(int id)
		{
			DriverLocationHelper.removeDriver(id);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
		[HttpPost]
		public HttpResponseMessage AddedLocation(CoordinatesDTO data)
		{
			DriverLocationHelper.addedLocation(data);
			return Request.CreateResponse(HttpStatusCode.OK, true);
		}
	}
}
