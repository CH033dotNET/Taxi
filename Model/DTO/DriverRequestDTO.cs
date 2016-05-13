using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class DriverRequestDTO
	{
		public int OrderId { get; set; }

		public string PeekPlace { get; set; }

		public string DropPlace { get; set; }

		public string WaitingTime { get; set; }

		public int DriverId { get; set; }
	}
}
