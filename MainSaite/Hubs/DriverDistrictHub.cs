using MainSaite.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSaite.Hubs
{
	[HubName("DriverDistrictHub")]
	public class DriverDistrictHub:Hub
	{
		static Dictionary<int, List<string>> DistrictMap = new Dictionary<int, List<string>>();

		[HubMethodName("joinDistrict")]
		public void JoinDistrict(int id)
		{
			if (!DistrictMap.Keys.Contains(id))
			{
				DistrictMap.Add(id, new List<string>());
			}
			DistrictMap[id].Add(Context.ConnectionId);
		}

		[HubMethodName("leaveDistrict")]
		public void LeaveDistrict(int id)
		{
			var current = DistrictMap.Where(c => c.Value.Contains(Context.ConnectionId)).FirstOrDefault().Key;
			DistrictMap[current].Remove(Context.ConnectionId);
			DistrictMap[id].Remove(Context.ConnectionId);
		}

		[HubMethodName("getDriversCount")]
		public List<DistrictCount> GetDriversCount()
		{
			return DistrictMap.Select(c => new DistrictCount() { Id = c.Key, Count = c.Value.Count }).ToList();
		}

		public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
		{
			var current = DistrictMap.Where(c => c.Value.Contains(Context.ConnectionId)).FirstOrDefault().Key;
			DistrictMap[current].Remove(Context.ConnectionId);
			return base.OnDisconnected(stopCalled);
		}
	}
}