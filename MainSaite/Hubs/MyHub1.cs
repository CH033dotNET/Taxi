using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MainSaite
{
	[HubName("MyHub1")]
	public class MyHub1 : Hub
	{
		[HubMethodName("Hello")]
		public void Hello()
		{
			Clients.All.dimine();
		}
	}
}