using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSaite
{
    [HubName("driversLocationHub")]
    public class driversLocationHub : Hub
	{
        [HubMethodName("addedLocation")]
		public static void addedLocation(Model.DTO.CoordinatesDTO coords)
		{
            var thisHub = GlobalHost.ConnectionManager.GetHubContext<driversLocationHub>();
            thisHub.Clients.All.locationUpdate(coords.Latitude, coords.Longitude, coords.AddedTime, coords.UserId);
		}

        [HubMethodName("addDriver")]
        public static void addDriver(int Id, double Latitude, double Longitude, DateTime time, string username)
        {
            var thisHub = GlobalHost.ConnectionManager.GetHubContext<driversLocationHub>();
            thisHub.Clients.All.driverStart(new
            {
                id = Id,
                latitude = Latitude,
                longitude = Longitude,
                startedTime = time,
                updateTime = time,
                name = username
            });
        }

        [HubMethodName("removeDriver")]
        public static void removeDriver(int id)
        {
            var thisHub = GlobalHost.ConnectionManager.GetHubContext<driversLocationHub>();
            thisHub.Clients.All.driverFinish(id);
        }
	}
}