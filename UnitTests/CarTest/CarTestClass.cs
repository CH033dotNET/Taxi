using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.DB;
using System.Collections.Generic;
using Common.Enum.CarEnums;
using System.Linq;

namespace UnitTests.CarTest
{
	[TestClass]
	public class CarTestClass
	{
		List<Car> testCars = new List<Car>();

		public CarTestClass()
		{
			#region Car initialization
			testCars.AddRange(new Car[]
				{
					new Car() { CarName = "Lada", 
						CarNickName = "21", 
						CarNumber = "AK9265AK", 
						CarOccupation = 4, 
						CarPetrolConsumption = 2, 
						CarClass = CarClassEnum.Normal, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92,
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2012,03,12,00,00,00) },
					new Car() { CarName = "Volga", 
						CarNickName = "13", 
						CarNumber = "KX3456KX", 
						CarOccupation = 4, 
						CarPetrolConsumption = 4, 
						CarClass = CarClassEnum.Normal, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92,
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2008,07,22,00,00,00) },
					new Car() { CarName = "Audi", 
						CarNickName = "41", 
						CarNumber = "HO8712HO", 
						CarOccupation = 4, 
						CarPetrolConsumption = 8, 
						CarClass = CarClassEnum.Normal, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Premium95,
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2011,11,05,00,00,00) },
					new Car() { CarName = "Mercedes Benz", 
						CarNickName = "134", 
						CarNumber = "IC6723IC", 
						CarOccupation = 4, 
						CarPetrolConsumption = 10, 
						CarClass = CarClassEnum.Normal, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Super98, 
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2013,02,02,00,00,00) },
					new Car() { CarName = "Volkswagen", 
						CarNickName = "231", 
						CarNumber = "HH5634HH", 
						CarOccupation = 4, 
						CarPetrolConsumption = 3, 
						CarClass = CarClassEnum.Universal, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92, 
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2010,03,24,00,00,00) },
					new Car() { CarName = "Opel",
						CarNickName = "245", 
						CarNumber = "KO0000KO", 
						CarOccupation = 4, 
						CarPetrolConsumption = 7, 
						CarClass = CarClassEnum.Lux, 
						CarState = CarStateEnum.Working, 
						CarPetrolType = CarPetrolEnum.Regular92, 
						OwnerId = 2,
						UserId = 2,
						CarManufactureDate = new DateTime(2009,05,23,00,00,00) }
				}); 
			#endregion
		}

		[TestMethod]
		public void TestFindMainCar()
		{
			int id = 2;
			bool isMainCarAvalable = false;
			var isAvalable = testCars.Any(x => x.UserId == id & x.isMain == true);
			if (isAvalable) { isMainCarAvalable = true; }
			Assert.IsFalse(isMainCarAvalable);
		}
		[TestMethod]
		public void TestOrdering()
		{
			int id = 2;
			string parameter = "name";
			var userCars = cases(parameter, id);
			Assert.IsNotNull(userCars);
			Assert.AreNotEqual(testCars, userCars);
		}

		private List<Car> cases(string parameter, int id)
		{
			List<Car> newCars = new List<Car>();
			switch (parameter)
			{
				case "name":
					newCars = testCars.OrderBy(x => x.CarName).Select(x => x).Where(x => x.UserId == id).ToList();
					//var userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				case "name_desc":
					newCars = testCars.OrderByDescending(x => x.CarName).Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				case "nickname":
					newCars = testCars.OrderBy(x => x.CarNickName).Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarNickName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				case "nickname_desc":
					newCars = testCars.OrderByDescending(x => x.CarNickName).Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarNickName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				case "number":
					newCars = testCars.OrderBy(x => x.CarNumber).Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarNumber).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				case "number_desc":
					newCars = testCars.OrderByDescending(x => x.CarNumber).Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarNumber).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
				default:
					newCars = testCars.Select(x => x).Where(x => x.UserId == id).ToList();
					//userCars = uOW.CarRepo.All.Where(x => x.UserId == id).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return newCars;
			}
		}
		[TestMethod]
		public void TestCarById()
		{
			int id = 2;
			int carId = 0;
			string status = "";
			var newlist = new Car();
			if (id <= 0 || carId <= 0) { status = "nocar"; }
			else
			{
				newlist = testCars.Where(s => s.Id == carId).FirstOrDefault();
				if (newlist == null) { status = "null"; }
				else status = "one";
			}
			//Assert.IsNotNull(newlist);
			Assert.AreSame(status, "nocar");
		}
	}
}
