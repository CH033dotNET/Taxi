using AutoMapper;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Manager
{

	public delegate void LocaTionEvent(CoordinatesDTO coords);
	public class CoordinatesManager : BaseManager, ICoordinatesManager
	{

		public event LocaTionEvent addedCoords;

		public CoordinatesManager(IUnitOfWork uOW)
			: base(uOW)
		{

		}
		public CoordinatesDTO AddCoordinates(CoordinatesDTO coordinates)
		{
			var coord = Mapper.Map<Coordinates>(coordinates);
			if (addedCoords != null) addedCoords(coordinates);


			//coord.Tarif = uOW.TariffExRepo.All.Where(t => t.Id == coord.TarifId).First();
			coord.User = uOW.UserRepo.All.Where(u => u.Id == coord.UserId).First();
			uOW.CoordinatesHistoryRepo.Insert(coord);
			uOW.Save();
			return coordinates;
		}

		public List<CoordinatesDTO> AddRangeCoordinates(List<CoordinatesDTO> coordinates, int orderId)
		{
			foreach (var t in coordinates)
				t.OrderId = orderId;
			var coord = coordinates.Select(x => Mapper.Map<Coordinates>(x)).ToList();
			foreach (var i in coord)
				uOW.CoordinatesHistoryRepo.Insert(i);
			uOW.Save();
			return coordinates;
		}
		/// <summary>
		/// Get records of Coordinates betwen fromTime and toTime 
		/// </summary>
		/// <param name="fromTime">Start Time</param>
		/// <param name="toTime">End Time</param>
		/// <returns>Records of Coordinates</returns>
		public IEnumerable<CoordinatesDTO> GetCoordinatesRange(DateTime fromTime, DateTime toTime)
		{
			var coords = uOW.CoordinatesHistoryRepo.Get().
				Where(s => (s.AddedTime > fromTime && s.AddedTime < toTime)).ToList();
			IEnumerable<CoordinatesDTO> coordDTO = new List<CoordinatesDTO>();
			foreach (var coordin in coords)
			{
				Mapper.Map<CoordinatesDTO>(coordin);
			}
			return coordDTO;
		}
		/// <summary>
		/// Remove records of Coordinates with added time earlier than date 
		/// </summary>
		/// <param name="date"></param>
		public void RemoveToDate(DateTime date)
		{
			var coordForDelete = uOW.CoordinatesHistoryRepo.Get()
				.Where(s => (s.AddedTime < date)).ToList();
			foreach (var coordinates in coordForDelete)
			{
				uOW.CoordinatesHistoryRepo.Delete(coordinates.Id);
			}
		}

		public CoordinatesDTO InitializeCoordinates(string Longitude, string Latitude, string Accuracy, int UserId)
		{
			return null;
		}

		public IEnumerable<Coordinates> GetCoordinatesByUserId(int userId)
		{
			var userCoordinates = uOW.CoordinatesHistoryRepo.Get().
				Where(x => x.UserId == userId).ToList();
			return userCoordinates;
		}

		public CoordinatesDTO GetCoordinatesByOrdeId(int ordeId)
		{
			var coord = uOW.CoordinatesHistoryRepo.Get().
				OrderByDescending(x => x.AddedTime).
				FirstOrDefault(x => x.OrderId == ordeId);


			var coordDTO = Mapper.Map<CoordinatesDTO>(coord);
			return coordDTO;
		}
	}
}
