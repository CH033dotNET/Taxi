using AutoMapper;
using BAL.Interfaces;
using Common.Enum.DriverEnum;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Linq;

namespace BAL.Manager
{
	public class DriverExManager : BaseManager, IDriverExManager
	{
		public DriverExManager(IUnitOfWork uOW) : base(uOW) { }

		public void AddDriverLocation(CoordinatesExDTO coordinate)
		{
			var location = Mapper.Map<CoordinatesEx>(coordinate);
			if (location.OrderId == 0)
				location.OrderId = null;
			uOW.CoordinatesExRepo.Insert(location);
			uOW.Save();
		}

	    public DriverLocationDTO[] GetFullLocations()
        {
           return uOW.CoordinatesExRepo.All //all coordinates
               .GroupBy(coordinates => coordinates.DriverId) //grouping all coordinates for each driver
               .Select(group => group.OrderByDescending(coordinates => coordinates.AddedTime).FirstOrDefault()) //select Latest coordinates from each group
               .Join(uOW.UserRepo.All, //add user data to coordinates
               coordinates => coordinates.DriverId, user => user.Id,
               (coordinates, user) => new //create new model whith nesessary field
               {
                   id = user.Id,
                   addedtime = coordinates.AddedTime,
                   latitude = coordinates.Latitude,
                   longitude = coordinates.Longitude,
                   name = user.UserName
               })
               .Join(uOW.WorkshiftHistoryRepo.All//join shifts
               .Where(shift => shift.WorkStarted != null & shift.WorkEnded == null)//select only current shifts
               , driver => driver.id, shift => shift.DriverId, (driver, shift) => new DriverLocationDTO()
               {
                   id = driver.id,
                   updateTime = driver.addedtime,
                   latitude = driver.latitude,
                   longitude = driver.longitude,
                   name = driver.name,
                   startedTime = shift.WorkStarted
               }
               ) // select drivers which now working
               .ToArray();
        }
	}
}
