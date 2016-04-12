using System;
using Model.DTO;

namespace BAL.Manager
{
	public interface ILocationManager
	{
		LocationDTO AddLocation(LocationDTO local);
		void DeleteLocation(int UserId);
		LocationDTO GetByUserId(int id);
		System.Collections.Generic.List<DriverDistrictInfoDTO> GetDriverDistrictInfo(int userId);
		LocationDTO UpdateLocation(LocationDTO local);
	}
}
