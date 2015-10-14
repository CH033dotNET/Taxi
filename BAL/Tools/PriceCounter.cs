using Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BAL.Manager;


namespace Common.Tools
{
	public class PriceCounter
	{
		private List<CoordinatesDTO> tempcoordinatesHistory = new List<CoordinatesDTO>();
		private List<CoordinatesDTO> coordinatesHistory = new List<CoordinatesDTO>();
		private List<TarifDTO> tarifes = new List<TarifDTO>();

		private CoordinatesDTO lastCoordinates;
		private TarifManager tarifManager;
		private TarifDTO currentTarif;
		private int currentTatifId = 0;
		private double distance;
		private double timePeriod;
		private double speed;
		private decimal finalPrice = 0;
		private decimal currentPrice = 0;
		private const double WAITINGCOSTSPEED = 5;

		public PriceCounter(TarifManager tarifManager)
		{
			this.tarifManager = tarifManager;
		}

		public PriceCounter(List<CoordinatesDTO> coordinatesHistory, List<TarifDTO> tarifes)
		{
			this.coordinatesHistory = coordinatesHistory;
			this.tarifes = tarifes;
			var item = tarifes.FirstOrDefault(tarif => tarif.id == coordinatesHistory[0].TarifId);
			if (item != null)
			{
				currentPrice = item.StartPrice;
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
					prevCoordinates = coordinatesHistory[iterator];
					iterator++;
					// distance (km)
					distance = GetDistance(prevCoordinates.Latitude, prevCoordinates.Longitude,
												coordinates.Latitude, coordinates.Longitude);
					//timePeriod (min)
					timePeriod = CountOfMinutes(prevCoordinates.AddedTime, coordinates.AddedTime);
					speed = distance / (timePeriod / 60); // km/h
					if (currentTatifId != coordinates.TarifId)
					{
						currentTarif = tarifes.FirstOrDefault(tarif => tarif.id == coordinates.TarifId);
						currentTatifId = coordinates.TarifId;
					}
					if (speed > 5)
					{

						currentPrice += (decimal)(timePeriod * (double)currentTarif.OneMinuteCost);
					}
					else
					{
						currentPrice += (decimal)(timePeriod * (double)currentTarif.WaitingCost);
					}
				}
				if (currentPrice > currentTarif.MinimalPrice)
				{
					return currentPrice;
				}
				else
				{
					return currentTarif.MinimalPrice;
				}
			}
			else
			{
				return 0;
			}
		}

		
		/// <summary>
		/// Temporary method wich calculate price between two coordinates and add to list coordinatesHistory
		/// </summary>
		/// <param name="coordinates"></param>
		/// <returns>Price between two coordinates</returns>
		public decimal CounterTick(CoordinatesDTO coordinates)
		{
			if (tempcoordinatesHistory.Count == 0)
			{
				tempcoordinatesHistory.Add(coordinates);
				return 0;
			}
			else
			{
				lastCoordinates = tempcoordinatesHistory.Last();
				// distance (km)
				distance = GetDistance(lastCoordinates.Latitude, lastCoordinates.Longitude, 
											coordinates.Latitude, coordinates.Longitude);
				//timePeriod (min)
				timePeriod = CountOfMinutes(lastCoordinates.AddedTime, coordinates.AddedTime);
				speed = distance / (timePeriod / 60); // km/h
				tempcoordinatesHistory.Add(coordinates);
				if (currentTatifId != coordinates.TarifId)
				{
					currentTarif = tarifManager.GetById(coordinates.TarifId);
					currentTatifId = coordinates.TarifId;
				}
				if (speed > 5)
				{
					
					currentPrice += (decimal)(timePeriod * (double)currentTarif.OneMinuteCost);
					return currentPrice;
				}
				else
				{
					currentPrice += (decimal)(timePeriod * (double)currentTarif.WaitingCost);
					return currentPrice;
				}
			}
		}

		public double CountOfMinutes(DateTime start, DateTime end)
		{
			double checkTime = ((end.Ticks - start.Ticks) / 600000000d);
			return (double)((end.Ticks - start.Ticks) / 600000000d);
		}

		private double GetDistance(double PreLatitude, double PreLongitude, double CurLatitude, double CurLongitude)
		{
			string reqestUrl = String.Format(
				"https://maps.googleapis.com/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}",
				PreLatitude, PreLongitude, CurLatitude, CurLongitude);
			HttpWebRequest request =
			(HttpWebRequest)WebRequest.Create(reqestUrl);

			request.Method = "GET";
			request.Accept = "application/json";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			StringBuilder output = new StringBuilder();
			output.Append(reader.ReadToEnd());
			dynamic jobject = JObject.Parse(output.ToString());
			int distant = (int)jobject.rows[0].elements[0].distance.value;

			response.Close();
			return distant / 1000;
		}

		public void StopCounter()
		{
			finalPrice = 0;
			currentPrice = 0;
			tempcoordinatesHistory.Clear();
		}
	}
}
