using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class CarManager : BaseManager
	{
		public CarManager(IUnitOfWork uOW) : base(uOW) { }

		// add new car to the repo
		public void addCar(CarDTO car)
		{
			var item = Mapper.Map<Car>(car);
			uOW.CarRepo.Insert(item);
			uOW.Save();
		}

		//delete cars by some Id
		public void deleteCarByID(int id)
		{
			Car car = uOW.CarRepo.GetByID(id);
			uOW.CarRepo.Delete(car);
			uOW.Save();
		}

		// get all cars in repo
		public IEnumerable<Car> getCars()
		{
			var carList = uOW.CarRepo.Get();
			return carList;
		}
		// must get list of cars for specific user
		public IEnumerable<CarDTO> getCarsByUserID(int id)
		{
			var userCars = uOW.CarRepo.Get().Where(s => s.UserId == id).Select(s => Mapper.Map<CarDTO>(s));
			//var userCarsDTO = Mapper.Map<CarDTO>(userCar);
			if (userCars != null)
			{
				return userCars;
			}
			return null;
		}
		// get specific car by it`s id
		public CarDTO GetCarByCarID(int id)
		{
			var userCar = uOW.CarRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			if (userCar != null)
			{
				return Mapper.Map<CarDTO>(userCar);
			}
			return null;
		}
	}
}
