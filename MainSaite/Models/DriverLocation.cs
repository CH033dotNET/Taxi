using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSaite.Models
{
	public class DriverLocation
	{
		public int id{get; set;}
		public double latitude { get; set; }
		public double longitude { get; set; }
		public DateTime updateTime { get; set; }
		public DateTime? startedTime { get; set; }
		public string name { get; set; }
	}
}
