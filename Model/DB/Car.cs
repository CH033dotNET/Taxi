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
		[Display(Name = "Car`s name: ")]
		public string CarName { get; set; }
		[Required]
		[Display(Name = "Car`s number: ")]
		public string CarNumber { get; set; }
		[Required]
		[Display(Name = "Car`s occupation: ")]
		public int CarOccupation { get; set; }
		[Required]
		[Display(Name = "Car`s class: ")]
		public CarClassEnum CarClass { get; set; }
		[Required]
		[Display(Name = "Car`s petrol type: ")]
		public string CarPetrolType { get; set; }
		[Required]
		[Display(Name = "Car`s petrol consumption: ")]
		public string CarPetrolConsumption { get; set; }
		[Required]
		[Display(Name = "Car`s manufaturing date: ")]
		public string CarManufactureDate { get; set; }
		[Required]
		[Display(Name = "Car`s state: ")]
		public CarStateEnum CarState { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
