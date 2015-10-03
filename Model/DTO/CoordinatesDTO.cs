using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class CoordinatesDTO
	{
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Accuracy { get; set; }

		public DateTime AddedTime { get; set; }

		public int UserId { get; set; }
	}
}
