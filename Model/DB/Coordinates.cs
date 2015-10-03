using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class Coordinates
	{
		[Key]
		public int Id { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Accuracy { get; set; }

		public DateTime AddedTime { get; set; }

		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual User User { get; set; }
	}
}
