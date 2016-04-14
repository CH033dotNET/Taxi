using Model.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class District
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MinLength(4, ErrorMessage = "Минимальная длинна - 4 символа")]
		public string Name { get; set; }

		public virtual List<Coordinate> Polygon { get; set; }

		public bool Deleted { get; set; }

		public District()
		{
			Polygon = new List<Coordinate>();
		}
	}
}
