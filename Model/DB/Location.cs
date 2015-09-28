using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Model.DB
{
	public class Location
	{

		[Key, ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User User { get; set; }

		[ForeignKey("DistrictId")]
		public virtual District District { get; set; }
		public int DistrictId { get; set; }

	}
}

