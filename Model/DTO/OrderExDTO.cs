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

		public string Address { get; set; }

		public OrderStatusEnum Status { get; set; }

		public int? DriverId { get; set; }

		public User Driver { get; set; }

		public int? CarId { get; set; }

		public Car Car { get; set; }

		public int WaitingTime { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yy HH':'mm}", ApplyFormatInEditMode = true)]
        public DateTime OrderTime { get; set; }

		public AddressFrom AddressFrom { get; set; }

		public ICollection<AddressTo> AddressesTo { get; set; }

		public AdditionallyRequirements AdditionallyRequirements { get; set; }

		public bool Route { get; set; }

		public int? UserId { get; set; }

		public string Name { get; set; }

		public string Phone { get; set; }

		public int Perquisite { get; set; }

		public int? ClientFeedbackId { get; set; }

		public Feedback ClientFeedback { get; set; }

		public int? DriverFeedbackId { get; set; }

		public Feedback DriverFeedback { get; set; }
	}
}
