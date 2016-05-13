using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class DriverLocationDTO
	{
		public int id { get; set; }

		public double latitude { get; set; }

		public double longitude { get; set; }

		public DateTime updateTime { get; set; }

		public DateTime? startedTime { get; set; }

		public string name { get; set; }
	}
}
