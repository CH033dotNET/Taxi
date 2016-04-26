using Common.Enum.CarEnums;
using Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class CarDTO
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 2)]
		public string CarName { get; set; }

		[Required]
		[StringLength(10, MinimumLength = 4)]
		public string CarNumber { get; set; }

		[Required]
		[Range(2, 20)]
		public int CarOccupation { get; set; }

		[Required]
		public CarClassEnum CarClass { get; set; }


		[Required]
		public CarPetrolEnum CarPetrolType { get; set; }


		[Required]
		[Range(1, 100)]
		public int CarPetrolConsumption { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[JsonConverter(typeof(CustomDateTimeConverter))]
		[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
		public DateTime CarManufactureDate { get; set; }

		[Required]
		public CarStateEnum CarState { get; set; }


		[Required]
		[StringLength(4, MinimumLength = 2)]
		public string CarNickName { get; set; }

		public bool isMain { get; set; }

		public int UserId { get; set; }

		public int OwnerId { get; set; }
	}
}
