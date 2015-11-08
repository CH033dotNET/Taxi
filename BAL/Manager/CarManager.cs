using AutoMapper;
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
		public string deleteCarByID(int id)
		{
			if (id <= 0)
			{
				return "Failure";
			}
			else
			{
				Car car = uOW.CarRepo.GetByID(id);
				uOW.CarRepo.Delete(car);
				uOW.Save();
				return "Success";
			}
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
			if (id <= 0 || id == null) { return null; }
			var userCars = uOW.CarRepo.All.Where(x => x.UserId == id).ToList().Select(s => Mapper.Map<CarDTO>(s));
			//var userCars = uOW.CarRepo.Get(s => s.UserId == id).Select(s => Mapper.Map<CarDTO>(s));
			if (userCars != null)
			{
				return userCars;
			}
			return null;
		}

		public IEnumerable<CarDTO> getCarsByUserID(int? id, string parameter)
		{
			if (id <= 0 || id == null) { return null; }
			switch (parameter)
			{
				case "name":
					var userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				case "name_desc":
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				case "nickname":
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarNickName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				case "nickname_desc":
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarNickName).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				case "number":
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderBy(x => x.CarNumber).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				case "number_desc":
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).OrderByDescending(x => x.CarNumber).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
				default:
					userCars = uOW.CarRepo.All.Where(x => x.UserId == id).ToList().Select(s => Mapper.Map<CarDTO>(s));
					return userCars;
			}
		}

		/// <summary>
		/// Gets a car which id is matching the value of input parameter
		/// </summary>
		/// <param name="id">input parameter</param>
		/// <returns></returns>
		public CarDTO GetCarByCarID(int id)
		{
			if (id <= 0)
			{
				return null;
			}
			else
			{
				var userCar = uOW.CarRepo.Get(s => s.Id == id).FirstOrDefault();
				if (userCar != null)
				{
					return Mapper.Map<CarDTO>(userCar);
				}
				return null;
			}
		}
		/// <summary>
		/// Manager method that is used to set specific car objects UserId property, 
		/// used to determine who is currently using specific car. 
		/// </summary>
		/// <param name="CarId">Input parameter that represents car object`s id.</param>
		/// <param name="NewCarUserId">Input paramenter that represents new car object`s id.</param>
		/// <returns></returns>
		public string GiveAwayCar(int CarId, int NewCarUserId)
		{
			if (CarId <= 0 || NewCarUserId <= 0)
			{
				return "Error";
			}
			var GiveAwayCar = uOW.CarRepo.Get(s => s.Id == CarId).FirstOrDefault();
			if (GiveAwayCar == null)
			{
				return "Error";
			}
			uOW.CarRepo.SetStateModified(GiveAwayCar);
			GiveAwayCar.UserId = NewCarUserId;
			GiveAwayCar.isMain = false;
			uOW.Save();
			return "Success";
		}
		/// <summary>
		/// Manager method that is used to change car`s isUsed property.
		/// </summary>
		/// <param name="carId">Input parameter that represents specific car object`s id.</param>
		/// <param name="userId">Input parameter that represents id of a current user.</param>
		/// <returns></returns>
		public string ChangeCarToMain(int carId, int userId)
		{
			if (carId <= 0 || userId <= 0)
			{
				return "Error";
			}
			// We need cars that are currently being used by current driver.
			var usedCar = uOW.CarRepo.Get(x => x.isMain == true & x.UserId == userId).FirstOrDefault(); // find a car with status set as true, which are used by our driver
			if (usedCar != null) // if such car exists
			{
				if (usedCar.Id == carId) // and if this car id match input parameter
				{
					return MakeACarMain(usedCar); // change this car`s id
				}
				else // if it`s not, if its other car, change its status to false and change status of car that we need
				{
					uOW.CarRepo.SetStateModified(usedCar);
					usedCar.isMain = false;
					uOW.Save();
					var message = MakeACarMain(carId, userId);
					return message;
				} 
			}
			else // if there are no cars with status as true
			{
				var message = MakeACarMain(carId, userId);
				return message;
			}
		}
		/// <summary>
		/// Private manager method taht is used to change isMain property of a specific car object`s property.
		/// </summary>
		/// <param name="carToChange">Input parameter that represents object that we need to change.</param>
		/// <returns></returns>
		private string MakeACarMain(Car carToChange)
		{
			try
			{
				uOW.CarRepo.SetStateModified(carToChange);
				carToChange.isMain = !carToChange.isMain;
				uOW.Save();
			}
			catch (Exception)
			{
				return "Error";
			}
			return "Success";
		}

		/// <summary>
		/// Private manager method taht is used to change isMain property of a specific car object`s property.
		/// </summary>
		/// <param name="carId">Input parameter that represents car object`s id.</param>
		/// <param name="userId">Input parameter that represents id of a current user.</param>
		/// <returns></returns>
		private string MakeACarMain(int carId, int userId)
		{
			var car = uOW.CarRepo.Get(x => x.Id == carId & x.UserId == userId).FirstOrDefault();
			if (car == null)
			{
				return "Nothing was found";
			}
			else
			{
				uOW.CarRepo.SetStateModified(car);
				car.isMain = !car.isMain;
				uOW.Save();

				return "Success";
			}

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
