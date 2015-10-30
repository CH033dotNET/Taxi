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
		private IUnitOfWork uOW; //???
		private ILocationManager locationManager;
		private IDistrictManager districtManager;

		public LocationController(ILocationManager locationManager, IUnitOfWork uOW, IDistrictManager districtManager)
		{
			this.locationManager = locationManager;
			this.uOW = uOW;
			this.districtManager = districtManager;
		}
	}
}
