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
	public class CoordinatesManager : BaseManager
	{
		public CoordinatesManager(IUnitOfWork uOW)
			: base(uOW)
		{

		}
		public CoordinatesDTO AddCoordinates(CoordinatesDTO coordinates)
		{
			var coord = Mapper.Map<Coordinates>(coordinates);
			uOW.CoordinatesHistoryRepo.Insert(coord);
			uOW.Save();
			return Mapper.Map<CoordinatesDTO>(coord);
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
		/// <summary>
		/// Initialize CoordinatesDTO from string parameters
		/// </summary>
		/// <param name="Longitude"></param>
		/// <param name="Latitude"></param>
		/// <param name="Accuracy"></param>
		/// <param name="UserId"></param>
		/// <returns>CoordinatesDTO</returns>
		public CoordinatesDTO InitializeCoordinates(string Longitude, string Latitude, string Accuracy, int UserId)
		{
			CoordinatesDTO coordinates = new CoordinatesDTO();
			try
			{
				coordinates.Latitude = double.Parse(Latitude);
				coordinates.Longitude = double.Parse(Longitude);
				coordinates.Accuracy = double.Parse(Accuracy);
			}
			catch (FormatException)
			{
				Latitude = Latitude.Replace('.', ',');
				Longitude = Longitude.Replace('.', ',');
				Accuracy = Accuracy.Replace('.', ',');
				coordinates.Latitude = double.Parse(Latitude);
				coordinates.Longitude = double.Parse(Longitude);
				coordinates.Accuracy = double.Parse(Accuracy);
			}
			coordinates.UserId = UserId;
			coordinates.AddedTime = DateTime.Now;
			return coordinates;
		}
	}
}
