using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class CoordinatesExDTO
	{
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public double Accuracy { get; set; }

		public DateTime AddedTime { get; set; }

		public int OrderId { get; set; }

		public OrderExDTO Order { get; set; }

		public int DriverId { get; set; }

		public virtual UserDTO Driver { get; set; }
	}
}
