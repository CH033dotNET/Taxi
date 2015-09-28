﻿using AutoMapper;
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

		//delete cars by car Id
		public void deleteCarByID(int? id)
		{
			Car car = uOW.CarRepo.GetByID(id);
			uOW.CarRepo.Delete(car);
			uOW.Save();
		}
		public CarDTO EditCar(CarDTO car)
		{
			var newCar = uOW.CarRepo.Get(s => s.Id == car.Id).First();
			if (newCar == null)
			{
				return null;
			}
			uOW.CarRepo.SetStateModified(newCar);
			newCar.CarName = car.CarName;
			newCar.CarNumber = car.CarNumber;
			newCar.CarOccupation = car.CarOccupation;
			newCar.CarClass = car.CarClass;
			newCar.CarPetrolType = car.CarPetrolType;
			newCar.CarPetrolConsumption = car.CarPetrolConsumption;
			newCar.CarManufactureDate = car.CarManufactureDate;
			newCar.CarState = car.CarState;
			uOW.Save();
			return Mapper.Map<CarDTO>(newCar);
		}

		// get all cars in repo
		public IEnumerable<CarDTO> getCars()
		{
			var carList = uOW.CarRepo.Get().Select(s => Mapper.Map<CarDTO>(s));
			return carList;
		}
		// must get list of cars for specific user
		public IEnumerable<CarDTO> getCarsByUserID(int? id)
		{
			var userCars = uOW.CarRepo.Get().Where(s => s.UserId == id).Select(s => Mapper.Map<CarDTO>(s));
			if (userCars != null)
			{
				return userCars;
			}
			return null;
		}
		// get specific car by it`s id
		public CarDTO GetCarByCarID(int? id)
		{
			if (id == 0)
			{
				return null;
			}
			var userCar = uOW.CarRepo.Get().Where(s => s.Id == id).FirstOrDefault();
			if (userCar != null)
			{
				return Mapper.Map<CarDTO>(userCar);
			}
			return null;
		}
		public IEnumerable<WorkshiftHistoryDTO> GetWorkingDrivers()
		{
			var workingUsers = uOW.WorkshiftHistoryRepo.Get().Where(s => s.WorkEnded == null & s.WorkStarted != null).Select(s => Mapper.Map<WorkshiftHistoryDTO>(s));
			if (workingUsers != null)
			{
				return workingUsers;
			}
			return null;
		}
		public WorkshiftHistoryDTO StartWorkEvent(int? id)
		{
			var newWorker = uOW.WorkshiftHistoryRepo.Get(s => s.DriverId == id).First();
			if (newWorker == null)
			{
				uOW.WorkshiftHistoryRepo.SetStateModified(newWorker);
				newWorker.DriverId = (int)id;
				newWorker.WorkStarted = DateTime.Now;
				//newWorker.WorkEnded = null;
				return Mapper.Map<WorkshiftHistoryDTO>(newWorker);
			}
			uOW.WorkshiftHistoryRepo.SetStateModified(newWorker);
			newWorker.WorkStarted = DateTime.Now;
			//newWorker.WorkEnded = null;
			uOW.Save();
			return Mapper.Map<WorkshiftHistoryDTO>(newWorker);
		}
	}
}
