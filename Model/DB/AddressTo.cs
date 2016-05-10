using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DB
{
	public class AddressTo
	{
		[Key]
		public int Id { get; set; }

		public string Address { get; set; }

		public string Building { get; set; }

		[ForeignKey("OrderEx")]
		public int OrderExId { get; set; }
		public virtual OrderEx OrderEx { get; set; }
	}
}
