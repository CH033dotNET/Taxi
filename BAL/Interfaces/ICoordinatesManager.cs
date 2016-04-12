using Model.DTO;
using System;
using System.Collections.Generic;
namespace BAL.Manager
{
	public interface ICoordinatesManager
	{
		event LocaTionEvent addedCoords;
		CoordinatesDTO AddCoordinates(CoordinatesDTO coordinates);
		IEnumerable<CoordinatesDTO> GetCoordinatesRange(DateTime fromTime, DateTime toTime);
		CoordinatesDTO InitializeCoordinates(string Longitude, string Latitude, string Accuracy, int UserId);
		IEnumerable<Model.DB.Coordinates> GetCoordinatesByUserId(int userId);
		List<CoordinatesDTO> AddRangeCoordinates(List<CoordinatesDTO> coordinates, int orderId);
		CoordinatesDTO GetCoordinatesByOrdeId(int ordeId);
		void RemoveToDate(DateTime date);
	}
}
