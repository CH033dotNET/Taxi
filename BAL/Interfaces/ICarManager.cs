using System;
namespace BAL.Manager
{
	public interface ICarManager
	{
		void addCar(Model.DTO.CarDTO car);
		void deleteCarByID(int? id);
		Model.DTO.CarDTO EditCar(Model.DTO.CarDTO car);
		string EndAllCurrentUserShifts(int id, string timeStop);
		void EndWorkShiftEvent(int? id);
		Model.DTO.CarDTO GetCarByCarID(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.CarDTO> getCars();
		System.Collections.Generic.IEnumerable<Model.DTO.CarDTO> getCarsByUserID(int? id);
		System.Collections.Generic.IEnumerable<Model.DTO.WorkshiftHistoryDTO> GetWorkingDrivers();
		bool GetWorkShiftsByWorkerId(int WorkerId);
		void GiveAwayCar(int CarId, int NewCarUserId);
		void StartWorkEvent(int? id, string TimeStart);
	}
}
