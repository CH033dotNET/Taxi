using Common.Enum.CarEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
	public class CarDTO
	{
		public int Id { get; set; }
		public string CarName { get; set; }
		public string CarNumber { get; set; }
		public int CarOccupation { get; set; }
		public CarClassEnum CarClass { get; set; }
		public string CarPetrolType { get; set; }
		public string CarPetrolConsumption { get; set; }
		public string CarManufactureDate { get; set; }
		public CarStateEnum CarState { get; set; }

		public virtual User User { get; set; }
		public int UserId { get; set; }
	}
}
