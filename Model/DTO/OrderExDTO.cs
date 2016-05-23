using System;
using System.Collections.Generic;
using Common.Enum;
using Model.DB;
using System.ComponentModel.DataAnnotations;

namespace Model.DTO
{
	public class OrderExDTO
	{
		public int Id { get; set; }

		public OrderStatusEnum Status { get; set; }

		public int? DriverId { get; set; }

		public User Driver { get; set; }

		public int? CarId { get; set; }

		public Car Car { get; set; }

		public int WaitingTime { get; set; }

		[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
		public decimal Price { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yy HH':'mm}", ApplyFormatInEditMode = true)]
        public DateTime OrderTime { get; set; }

		public AddressFromDTO AddressFrom { get; set; }

		public List<AddressToDTO> AddressesTo { get; set; }

		public AdditionallyRequirementsDTO AdditionallyRequirements { get; set; }

		public bool Route { get; set; }

		public int? UserId { get; set; }

		public string Name { get; set; }

		public string Phone { get; set; }

		public int Perquisite { get; set; }

		public int? ClientFeedbackId { get; set; }

		public Feedback ClientFeedback { get; set; }

		public int? DriverFeedbackId { get; set; }

		public Feedback DriverFeedback { get; set; }

		public string FullAddressFrom { get; set; }

		public string FullAddressTo { get; set; }
	}
}
