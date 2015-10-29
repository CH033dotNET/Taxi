using System;
namespace BAL.Manager
{
	public interface ICoordinatesManager
	{
		event LocaTionEvent addedCoords;
		Model.DTO.CoordinatesDTO AddCoordinates(Model.DTO.CoordinatesDTO coordinates);
		System.Collections.Generic.IEnumerable<Model.DTO.CoordinatesDTO> GetCoordinatesRange(DateTime fromTime, DateTime toTime);
		Model.DTO.CoordinatesDTO InitializeCoordinates(string Longitude, string Latitude, string Accuracy, int UserId);
		void RemoveToDate(DateTime date);
	}
}
