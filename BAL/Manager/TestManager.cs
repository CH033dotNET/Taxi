using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class TestManager
	{
		public CarDTO GetCar()
		{
			return new CarDTO() { CarName = "NYasha", CarNickName = "0000", CarNumber = "9999999", CarOccupation = 2, CarPetrolConsumption = 2, Id = 2, OwnerId = 2, UserId = 2, CarManufactureDate = DateTime.Now, CarClass = Common.Enum.CarEnums.CarClassEnum.Normal, CarPetrolType = Common.Enum.CarEnums.CarPetrolEnum.Regular92, CarState = Common.Enum.CarEnums.CarStateEnum.Working };
		}
	}
}
