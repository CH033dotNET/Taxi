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
		[MaxLength(30, ErrorMessage = "Максимальная длинна - 30 символов")]
		[MinLength(3, ErrorMessage = "Минимальная длинна - 3 символов")]
		public string CarName { get; set; }
		[Required]
		public string CarNumber { get; set; }
		[Required]
		public int CarOccupation { get; set; }
		[Required]
		public CarClassEnum CarClass { get; set; }
		[Required]
		public string CarPetrolType { get; set; }
		[Required]
		public string CarPetrolConsumption { get; set; }
		[Required]
		public string CarManufactureDate { get; set; }
		[Required]
		public CarStateEnum CarState { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
