using System;
namespace BAL.Manager
{
	 public interface IDriverManager
	{
		Model.DTO.DriverLocation[] GetFullLocations();
	}
}
