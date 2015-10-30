using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using Model.DTO;
using MainSaite.Helpers;

namespace TaxiAPI.Controllers
{
    public class DriverController : ApiController
    {
		private ILocationManager locationManager;
		private ICarManager carManager;
		private ICoordinatesManager coordinatesManager;
		private IUserManager userManager;
		private IDriverLocationHelper driverLocationHelper;
		public DriverController(ILocationManager locationManager, ICarManager carManager, ICoordinatesManager coordinatesManager, IUserManager userManager, IDriverLocationHelper driverLocationHelper)
		{
			this.locationManager = locationManager;
			this.carManager = carManager;
			this.coordinatesManager = coordinatesManager;
			this.userManager = userManager;
			this.driverLocationHelper = driverLocationHelper;
			this.coordinatesManager.addedCoords += this.driverLocationHelper.addedLocation;
		}
    }
}
