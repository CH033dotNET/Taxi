using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	[Table("DistrictCoordinates")]
	public class Coordinate
	{
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Index { get; set; }

		public District District { get; set; }

		public int DistrictId { get; set; }
	}
}
