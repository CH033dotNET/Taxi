using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MainSaite.Helpers;
using Model.DTO;
using Common.Tools;
using BAL.Manager;

namespace TaxiAPI.Controllers
{
    public class ClientServiceController : ApiController
    {
		private ITarifManager tarifManager;
		private IOrderManager orderManager;
		private ICoordinatesManager coordinatesManager;
		private IDriverLocationHelper driverLocationHelper;
		public ClientServiceController(ITarifManager tarifManager, IOrderManager orderManager, ICoordinatesManager coordinatesManager, IDriverLocationHelper driverLocationHelper)
		{
			this.orderManager = orderManager;
			this.tarifManager = tarifManager;
			this.coordinatesManager = coordinatesManager;
			this.driverLocationHelper = driverLocationHelper;
			this.coordinatesManager.addedCoords += this.driverLocationHelper.addedLocation;
		}
    }
}
