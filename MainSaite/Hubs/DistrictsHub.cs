using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Hubs
{
	[HubName("DistrictsHub")]
	public class DistrictsHub : Hub
	{
		[HubMethodName("swap")]
		public void Swap(int newDistrct, int oldDistrict)
		{
			Clients.All.swap(newDistrct, oldDistrict);
		}
	}
}