using System;
namespace BAL.Manager
{
	public interface ILocationManager
	{
		Model.DTO.LocationDTO AddLocation(Model.DTO.LocationDTO local);
		void DeleteLocation(int UserId);
		Model.DTO.LocationDTO GetByUserId(int id);
		System.Collections.Generic.List<Model.DTO.DriverDistrictInfoDTO> GetDriverDistrictInfo(int userId);
		Model.DTO.LocationDTO UpdateLocation(Model.DTO.LocationDTO local);
	}
}
