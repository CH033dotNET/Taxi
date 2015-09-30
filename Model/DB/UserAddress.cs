using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class UserAddress
	{
		[Key]
		public int AddressId { get; set; }

		[Required]
		[StringLength(50)]
		public string City { get; set; }

		[Required]
		[StringLength(100)]
		public string Street { get; set; }

		[Required]
		[StringLength(10)]
		public string Number { get; set; }

		[Required]
		[StringLength(500)]
		public string Comment { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
