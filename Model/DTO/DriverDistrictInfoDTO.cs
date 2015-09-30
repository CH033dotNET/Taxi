using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class DriverDistrictInfoDTO
	{
		public int DriverCount { get; set; }
		public string DistrictName { get; set; }
		public int DistrictId { get; set; }
		public List<string> Drivers { get; set; }
	}
}
