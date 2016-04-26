 using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriverSite.Hubs
{
    [HubName("driverLocationHub")]
    public class DriverLocationHub : Hub
    {
        //static ICollection<SignalRUser> drivers = new List<SignalRUser>();

        [HubMethodName("addDriver")]
        public void addDriver(int id, double lat, double lng, string name)
        {
            IHubContext conntext = GlobalHost.ConnectionManager.GetHubContext<DriverLocationHub>();
            conntext.Clients.All.addDriver(id, Context.ConnectionId, lat, lng, name);
        }


        [HubMethodName("updateDriverCoord")]
        public void updateDriver(int id, int connectionId, double lat, double lng, string name)
        {
            IHubContext conntext = GlobalHost.ConnectionManager.GetHubContext<DriverLocationHub>();
            conntext.Clients.All.updateDriverCoord(id,this.Context.ConnectionId, lat, lng, name);
        }


        [HubMethodName("remove")]
        public void removeDrivr(string id)
        {
            IHubContext conntext = GlobalHost.ConnectionManager.GetHubContext<DriverLocationHub>();

            conntext.Clients.All.remove(id);
        }


        /*[HubMethodName("connectUser")]
        public void ConnectUser()
        {
            string connectionId = Context.ConnectionId;

            var currentUser = new SignalRUser() { ConnectionId = connectionId};
            if (!drivers.Any(x => x.ConnectionId == connectionId))
            {
                drivers.Add(currentUser);
            }
        }*/
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            IHubContext conntext = GlobalHost.ConnectionManager.GetHubContext<DriverLocationHub>();
            this.removeDrivr(this.Context.ConnectionId);
            return base.OnDisconnected(true);
        }


    }
}