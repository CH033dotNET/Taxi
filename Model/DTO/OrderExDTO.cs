using System;
using System.Collections.Generic;
using Common.Enum;
using Model.DB;

namespace Model.DTO
{
	public class OrderExDTO
	{
		public int Id { get; set; }

		public string Address { get; set; }

		public OrderStatusEnum Status { get; set; }

		public int? DriverId { get; set; }

		public User Driver { get; set; }

		public int WaitingTime { get; set; }

        public DateTime OrderTime { get; set; }

		public AddressFrom AddressFrom { get; set; }

		public ICollection<AddressTo> AddressesTo { get; set; }

		public AdditionallyRequirements AdditionallyRequirements { get; set; }

		public bool Route { get; set; }

		public int? UserId { get; set; }

		public string Name { get; set; }

		public string Phone { get; set; }

		public int Perquisite { get; set; }
	}
}
