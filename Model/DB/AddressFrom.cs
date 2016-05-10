using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DB
{
	public class AddressFrom
	{
		[Key, ForeignKey("OrderEx")]
		public int Id { get; set; }

		[Required]
		public string Address { get; set; }

		public string Building { get; set; }

		public string Entrance { get; set; }

		public string Note { get; set; }

		public virtual OrderEx OrderEx { get; set; }
	}
}
