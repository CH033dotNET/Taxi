using Model.DB;

namespace Model.DTO
{
	public class AddressToDTO
	{
		public int Id { get; set; }

		public string Address { get; set; }

		public string Building { get; set; }

		public virtual OrderEx OrderEx { get; set; }
	}
}
