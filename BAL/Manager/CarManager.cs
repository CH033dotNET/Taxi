﻿using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{
	public class CarManager : BaseManager, ICarManager
	{
		public CarManager(IUnitOfWork uOW) :base(uOW)
		{

		}

		/// <summary>
		/// Adds new Car to repository
		/// </summary>
		/// <param name="car">input object</param>
		public void addCar(CarDTO car)
		{
			var item = Mapper.Map<Car>(car);
			uOW.CarRepo.Insert(item);
			uOW.Save();
		}

		/// <summary>
		/// Finds car by it`s id and deletes it
		/// </summary>
		/// <param name="id">value that represents car id</param>
		public void deleteCarByID(int? id)
		{
			Car car = uOW.CarRepo.GetByID(id);
			uOW.CarRepo.Delete(car);
			uOW.Save();
		}

		/// <summary>
		/// Finds a car entry in repository which id is equal to input object id
		/// </summary>
		/// <param name="car">input object</param>
		/// <returns></returns>
		public CarDTO EditCar(CarDTO car)
		{
			var newCar = uOW.CarRepo.Get(s => s.Id == car.Id).FirstOrDefault();
			if (newCar == null)
			{
				return null;
			}
			var result = SetCarStateModified(newCar, car);
			return result;
		}
		/// <summary>
		/// Edits car object using values from second input object and maps edited object to another type
		/// </summary>
		/// <param name="newCarObj">object that must be edited</param>
		/// <param name="inputCarObj">object which values are used for editing</param>
		/// <returns></returns>
		private CarDTO SetCarStateModified(Car newCarObj, CarDTO inputCarObj)
		{
			uOW.CarRepo.SetStateModified(newCarObj);
			newCarObj.CarName = inputCarObj.CarName;
			newCarObj.CarNumber = inputCarObj.CarNumber;
			newCarObj.CarOccupation = inputCarObj.CarOccupation;
			newCarObj.CarClass = inputCarObj.CarClass;
			newCarObj.CarPetrolType = inputCarObj.CarPetrolType;
			newCarObj.CarPetrolConsumption = inputCarObj.CarPetrolConsumption;
			newCarObj.CarManufactureDate = inputCarObj.CarManufactureDate;
			newCarObj.CarState = inputCarObj.CarState;
			newCarObj.CarNickName = inputCarObj.CarNickName;
			uOW.Save();
			return Mapper.Map<CarDTO>(newCarObj);
		}

		/// <summary>
		/// Gets all car entries from repository
		/// </summary>
		/// <returns></returns>
		public IEnumerable<CarDTO> getCars()
		{
			var carList = uOW.CarRepo.Get().Select(s => Mapper.Map<CarDTO>(s));
			return carList;
		}

		/// <summary>
		/// Gets list of cars which user`s id is equal to the value of input parameter 
		/// </summary>
		/// <param name="id"></param>
		/// <returns>parameter representing users id</returns>
		public IEnumerable<CarDTO> getCarsByUserID(int? id)
		{
			//var userCars = uOW.CarRepo.Get().Where(s => s.UserId == id).Select(s => Mapper.Map<CarDTO>(s));
			var userCars = uOW.CarRepo.Get(s => s.UserId == id).Select(s => Mapper.Map<CarDTO>(s));
			if (userCars != null)
			{
				return userCars;
			}
			return null;
		}

		/// <summary>
		/// Gets a car which id is matching the value of input parameter
		/// </summary>
		/// <param name="id">input parameter</param>
		/// <returns></returns>
		public CarDTO GetCarByCarID(int? id)
		{
			var userCar = uOW.CarRepo.Get(s => s.Id == id).FirstOrDefault();
			if (userCar != null)
			{
				return Mapper.Map<CarDTO>(userCar);
			}
			return null;
		}

		public void GiveAwayCar(int CarId, int NewCarUserId)
		{
			var GiveAwayCar = uOW.CarRepo.Get(s => s.Id == CarId).FirstOrDefault();
			if (GiveAwayCar == null)
			{
				throw new NotImplementedException(); // ------------!!!
			}
			uOW.CarRepo.SetStateModified(GiveAwayCar);
			GiveAwayCar.UserId = NewCarUserId;
			uOW.Save();
		}

		/// <summary>
		/// Gets list of workers from a pero by specific criteria. If criteria is not matching returns null  
		/// </summary>
		/// <returns></returns>
		public IEnumerable<WorkshiftHistoryDTO> GetWorkingDrivers()
		{
			var workingUsers = uOW.WorkshiftHistoryRepo.Get(s => s.WorkEnded == null & s.WorkStarted != null).Select(s => Mapper.Map<WorkshiftHistoryDTO>(s));
			if (workingUsers != null)
			{
				return workingUsers;
			}
			return null;
		}
		/// <summary>
		/// Searches for uncompleted WorkShifts for driver with specific Id. If they are avialable, returns true, if not - false.
		/// </summary>
		/// <param name="WorkerId">drivers id</param>
		/// <returns></returns>
		public bool GetWorkShiftsByWorkerId(int WorkerId)
		{
			var uncompletedShifts = uOW.WorkshiftHistoryRepo.Get(s => s.WorkEnded == null & s.WorkStarted != null & s.DriverId == WorkerId).Any();
			if (uncompletedShifts) { return true; }
			else { return false; }
		}

		/// <summary>
		/// Starting workshift for a driver which id is matching input parameter`s value
		/// </summary>
		/// <param name="id">input parameter</param>
		public void StartWorkEvent(int? id, string TimeStart)
		{
			var worker = uOW.WorkshiftHistoryRepo.Get(s => s.DriverId == id).LastOrDefault(); // get the last entry for a current user
			var mappedworker = Mapper.Map<WorkshiftHistoryDTO>(worker);
			if (mappedworker == null) // if entry is empty (if no entry at all)
			{
				NewWorkerShift(id); // creating new entry (starting workshift)
			}
			else if (worker.WorkEnded == null) // if that last entry is unfinished (no end of workshift)
			{
				//var dbworker = Mapper.Map<WorkshiftHistory>(mappedworker);
				uOW.WorkshiftHistoryRepo.SetStateModified(worker); // finishing that shift 
				worker.WorkEnded = DateTime.ParseExact(TimeStart, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
				uOW.Save();
				NewWorkerShift(id); // starting new shift.
			}
			else // if entries are exist but all finished - create new entry
			{
				NewWorkerShift(id);
			}

		}

		/// <summary>
		/// Creates new workshift entry. WorkEnded property is null 
		/// </summary>
		/// <param name="id"></param>
		private void NewWorkerShift(int? id)
		{
			var newWorker = new WorkshiftHistoryDTO();
			newWorker.DriverId = (int)id;
			newWorker.WorkStarted = DateTime.Now;
			newWorker.WorkEnded = null;
			var mapWorker = Mapper.Map<WorkshiftHistory>(newWorker);
			uOW.WorkshiftHistoryRepo.Insert(mapWorker);
			uOW.Save();
		}

		/// <summary>
		/// Ends workshift entry which driver`s id is matching input parameters value
		/// </summary>
		/// <param name="id">input parameter</param>
		public void EndWorkShiftEvent(int? id)
		{
			var worker = uOW.WorkshiftHistoryRepo.Get(s => s.DriverId == id).LastOrDefault();
			uOW.WorkshiftHistoryRepo.SetStateModified(worker);
			worker.WorkEnded = DateTime.Now;
			uOW.Save();
		}

		/// <summary>
		/// Finishes current driver`s workshift, plus ends all user`s unfinished shifts
		/// </summary>
		/// <param name="id">user`s id</param>
		/// <param name="timeStop"></param>
		public string EndAllCurrentUserShifts(int id, string timeStop)
		{
			string message = "";
			var isWorker = uOW.WorkshiftHistoryRepo.Get(s => s.DriverId == id & s.WorkEnded == null).Any();
			if (isWorker)
			{
				var worker = uOW.WorkshiftHistoryRepo.Get(s => s.DriverId == id & s.WorkEnded == null);
				foreach (var times in worker)
				{
					uOW.WorkshiftHistoryRepo.SetStateModified(times);
					times.WorkEnded = DateTime.ParseExact(timeStop, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
					uOW.Save();
					message = "Session was closed";
					return message;
				}
			}
			else
			{
				message = "There are no pending sessions to close";
				return message;
			}
			return message;
		}
	}
}
