using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DB;
using Model.DTO;
using DAL.Interface;

namespace BAL.Manager
{
    public class DriverManager :BaseManager, IDriverManager
    {

        public DriverManager(IUnitOfWork uOW)
			:base(uOW)
        {

        }
        public DriverLocation[] GetFullLocations()
        {
            return uOW.CoordinatesHistoryRepo.All //all coordinates
                .GroupBy(coordinates => coordinates.UserId) //grouping all coordinates for each user
                .Select(group => group.OrderByDescending(coordinates => coordinates.AddedTime).FirstOrDefault()) //select Latest coordinates from each group
                .Join(uOW.UserRepo.All, //add user data to coordinates
                coordinates => coordinates.UserId, user => user.Id,
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
                , driver => driver.id, shift => shift.DriverId, (driver, shift) => new DriverLocation()
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
