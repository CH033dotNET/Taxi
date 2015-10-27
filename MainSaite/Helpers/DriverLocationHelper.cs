using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MainSaite.Hubs;

namespace MainSaite.Helpers
{
    public class DriverLocationHelper
    {
        private IHubContext Contest = GlobalHost.ConnectionManager.GetHubContext<DriversLocationHub>();

        public void addedLocation(Model.DTO.CoordinatesDTO coords)
        {
            var user = HttpContext.Current.Session["User"];
            Contest.Clients.All.locationUpdate(coords.Latitude, coords.Longitude, coords.AddedTime, coords.UserId);
        }

        public void addDriver(int Id, double Latitude, double Longitude, DateTime time, string username)
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

        public void removeDriver(int id)
        {
            Contest.Clients.All.driverFinish(id);
        }
    }
}