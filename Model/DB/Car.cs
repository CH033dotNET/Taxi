using Common.Enum.CarEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DB
{
	public class Car
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string CarName { get; set; }
		[Required]
		[StringLength(12, MinimumLength = 3)]
		public string CarNumber { get; set; }
		[Required]
		[Range(2,20)]
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
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime CarManufactureDate { get; set; }
		[Required]
		public CarStateEnum CarState { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
