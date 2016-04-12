using System;
using System.Linq;
using System.Collections.Generic;
using Model.DTO;

namespace BAL.Manager
{
	public interface ICarManager
	{
		void addCar(CarDTO car);
		string deleteCarByID(int id);
		CarDTO EditCar(CarDTO car);
		string EndAllCurrentUserShifts(int id, string timeStop);
		void EndWorkShiftEvent(int? id);
		CarDTO GetCarByCarID(int id);
		IEnumerable<CarDTO> getCars();
		IEnumerable<CarDTO> getCarsByUserID(int? id);
		IEnumerable<CarDTO> getCarsByUserID(int? id, string parameter);
		IEnumerable<WorkshiftHistoryDTO> GetWorkingDrivers();
		bool GetWorkShiftsByWorkerId(int WorkerId);
		string GiveAwayCar(int CarId, int NewCarUserId);
		void StartWorkEvent(int? id, string TimeStart);

		bool FindMainCar(int id);

		string ChangeCarToMain(int carId, int userId);
	}
}
