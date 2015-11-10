using System;
namespace MainSaite.Helpers
{
	public interface IDriverLocationHelper
	{
		void addDriver(int Id, double Latitude, double Longitude, DateTime time, string username);
		void addedLocation(Model.DTO.CoordinatesDTO coords);
		void removeDriver(int id);
        void removeDriverFromUserPage(int id);
        void addOnUserPageDriver(int Id, double Latitude, double Longitude, DateTime time, string username);
	}
}
