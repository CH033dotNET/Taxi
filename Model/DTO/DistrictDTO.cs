using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class DistrictDTO
	{
		public int Id { get; set; }

		[Required]
		[MinLength(4, ErrorMessage = "Минимальная длинна - 4 символа")]
		public string Name { get; set; }

		public List<CoordinateDTO> Coordinates { get; set; }

		public bool IsFolder { get; set; }

		public int? ParentId { get; set; }

		public bool Deleted { get; set; }

		public DistrictDTO()
		{
			Coordinates = new List<CoordinateDTO>();
		}
	}
}
