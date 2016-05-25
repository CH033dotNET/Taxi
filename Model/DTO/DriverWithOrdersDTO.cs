namespace Model.DTO
{
	public class DriverWithOrdersDTO
	{
		public UserDTO Driver { get; set; }

		public int OrdersCount { get; set; }

		public string Image { get; set; }

		public string Name { get; set; }
	}
}
