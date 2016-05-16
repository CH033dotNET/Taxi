using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MainSaite.Hubs;
using Model.DTO;

namespace MainSaite.Helpers
{
    public class DriverLocationHelper : IDriverLocationHelper
    {
        private static IHubContext Contest = GlobalHost.ConnectionManager.GetHubContext<DriversLocationHub>();

		public static void addedLocation(Model.DTO.CoordinatesExDTO coords)
        {
			Contest.Clients.All.locationUpdate(coords.Latitude, coords.Longitude,((coords.AddedTime - new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond), coords.DriverId);
        }

		public static void removeDriver(int id)
		{
			Contest.Clients.All.driverFinish(id);
		}

		public static void addDriver(DriverLocationDTO data)
		{
			Contest.Clients.All.driverStart(data);
		}

		public static void addDriver(int Id, double Latitude, double Longitude, DateTime time, string username)
        {
            Contest.Clients.All.driverStart(new
            {
                id = Id,
                latitude = Latitude,
                longitude = Longitude,
                startedTime = time,
                updateTime = time,
                name = username
            });
        }

        public void addOnUserPageDriver(int Id, double Latitude, double Longitude, DateTime time, string username)
        {
            Contest.Clients.All.driverStartOnUserPage(new
            {
                id = Id,
                latitude = Latitude,
                longitude = Longitude,
                startedTime = time,
                updateTime = time,
                name = username
            });
        }

        public void removeDriverFromUserPage(int id)
        {
            Contest.Clients.All.driverFinishUserPage(id);
        }
    }
}