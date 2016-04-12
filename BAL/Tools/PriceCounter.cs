using Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Common.Tools
{
	public class PriceCounter
	{
		private List<CoordinatesDTO> coordinatesHistory = new List<CoordinatesDTO>();
		private List<TarifDTO> tarifes = new List<TarifDTO>();

		private TarifDTO currentTarif;
		private int currentTatifId = 0;
		private double distance;
		public double finalDistance { get; private set; }
		private double timePeriod;
		private double speed;
		private decimal currentPrice = 0;
		private double preLongDist;
		private const double WAITINGCOSTSPEED = 5;
		private const int EARTHRADIUD = 6371;
		private const int ONEDEGREELATITUDE = 111;

		public PriceCounter()
		{ }

		public PriceCounter(List<CoordinatesDTO> coordinatesHistory, List<TarifDTO> tarifes)
		{
			this.coordinatesHistory = coordinatesHistory;
			this.tarifes = tarifes;
			if (coordinatesHistory.Count > 0)
			{
				var item = tarifes.FirstOrDefault(tarif => tarif.id == coordinatesHistory[0].TarifId);
				if (item != null)
				{
					currentPrice = item.StartPrice;
				}
			}
		}
		/// <summary>
		/// Calculates price by list of coordinates coordinatesHistory
		/// </summary>
		/// <returns>price</returns>
		public decimal CalcPrice()
		{
			if (coordinatesHistory.Count > 1)
			{
				int iterator = 0;
				CoordinatesDTO prevCoordinates;
				foreach (CoordinatesDTO coordinates in coordinatesHistory)
				{
					if (iterator == 0)
					{
						iterator++;
						continue;
					}
					prevCoordinates = coordinatesHistory[iterator - 1];
					iterator++;

					// distance (km)
					distance = GetDistance(prevCoordinates.Latitude, prevCoordinates.Longitude,
												coordinates.Latitude, coordinates.Longitude);
					finalDistance += distance;
					//timePeriod (min)
					timePeriod = CountOfMinutes(prevCoordinates.AddedTime, coordinates.AddedTime);
					speed = distance / (timePeriod / 60); // km/h
					if (currentTatifId != coordinates.TarifId)
					{
						currentTarif = tarifes.FirstOrDefault(tarif => tarif.id == coordinates.TarifId);
						currentTatifId = coordinates.TarifId;
					}
					if (speed > WAITINGCOSTSPEED)
					{

						currentPrice += (decimal)(timePeriod * (double)currentTarif.OneMinuteCost);
					}
					else
					{
						currentPrice += (decimal)(timePeriod * (double)currentTarif.WaitingCost);
					}
				}

				return currentPrice;

			}
			else
			{
				return currentPrice;
			}
		}


		/// <summary>
		/// Calculate namber of minutes between two timepoints
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public double CountOfMinutes(DateTime start, DateTime end)
		{
			double checkTime = ((end.Ticks - start.Ticks) / 600000000d);
			return (double)((end.Ticks - start.Ticks) / 600000000d);
		}

		/// <summary>
		/// Calculate count of kilometers between two geogragic points
		/// </summary>
		/// <param name="PreLatitude">Previous</param>
		/// <param name="PreLongitude"></param>
		/// <param name="CurLatitude"></param>
		/// <param name="CurLongitude"></param>
		/// <returns>distance(km)</returns>
		private double GetDistance(double PreLatitude, double PreLongitude, double CurLatitude, double CurLongitude)
		{
			//the number of kilometres in one degree of Longitude in dependence of Latitude
			preLongDist = EARTHRADIUD * (Math.PI / 180) * Math.Cos(PreLatitude * Math.PI / 180);

			return Math.Sqrt((Math.Pow(ONEDEGREELATITUDE * Math.Abs(PreLatitude - CurLatitude), 2)) +
							 (Math.Pow(preLongDist * Math.Abs(PreLongitude - CurLongitude), 2)));
		}
	}
}
