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
		public void StartWorkEvent(int? id)
		{
			var worker = uOW.WorkshiftHistoryRepo.Get().Where(s => s.DriverId == id).LastOrDefault(); // get the last entry for a current user
			var mappedworker = Mapper.Map<WorkshiftHistoryDTO>(worker);
			if (mappedworker == null) // if entry is empty (if no entry at all)
			{
				NewWorkerShift(id); // creating new entry (starting workshift)
			}
			else if (worker.WorkEnded == null) // if that last entry is unfinished (no end of workshift)
			{
				//var dbworker = Mapper.Map<WorkshiftHistory>(mappedworker);
				uOW.WorkshiftHistoryRepo.SetStateModified(worker); // finishing that shift 
				worker.WorkEnded = DateTime.Now;
				uOW.Save();
				NewWorkerShift(id); // starting new shift.
			}
			else // if entries are exist but all finished - create new entry
			{
				NewWorkerShift(id);
			}
			
		}
		public void NewWorkerShift(int? id)
		{
			var newWorker = new WorkshiftHistoryDTO();
			newWorker.DriverId = (int)id;
			newWorker.WorkStarted = DateTime.Now;
			newWorker.WorkEnded = null;
			var mapWorker = Mapper.Map<WorkshiftHistory>(newWorker);
			uOW.WorkshiftHistoryRepo.Insert(mapWorker);
			uOW.Save();
		}
		public void EndWorkShiftEvent(int? id)
		{
			var worker = uOW.WorkshiftHistoryRepo.Get().Where(s => s.DriverId == id).Last();
			uOW.WorkshiftHistoryRepo.SetStateModified(worker);
			worker.WorkEnded = DateTime.Now;
			uOW.Save();
		}
		/// <summary>
		/// Finishes current driver`s workshift, plus ends all user`s unfinished shifts
		/// </summary>
		/// <param name="id">user`s id</param>
		public void EndAllCurrentUserShifts(int id)
		{
			var worker = uOW.WorkshiftHistoryRepo.Get().Where(s => s.DriverId == id).Select(s => s);
			foreach (var times in worker)
			{
				uOW.WorkshiftHistoryRepo.SetStateModified(times);
				times.WorkEnded = DateTime.Now;
				uOW.Save();
			}
		}
	}
}
