using Model.DB;

namespace Model.DTO
{
	public class AddressFromDTO
	{
		public int Id { get; set; }

		public string Address { get; set; }

		public string Building { get; set; }

		public string Entrance { get; set; }

		public string Note { get; set; }

		public virtual OrderEx OrderEx { get; set; }
	}
}
