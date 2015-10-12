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
		private List<CoordinatesDTO> coordinatesHistory = new List<CoordinatesDTO>();
		private CoordinatesDTO lastCoordinates;
		private TarifManager tarifManager;
		private TarifDTO currentTarif;
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

		public decimal CounterTick(CoordinatesDTO coordinates)
		{
			if (coordinatesHistory.Count == 0)
			{
				coordinatesHistory.Add(coordinates);
				return 0;
			}
			else
			{
				lastCoordinates = coordinatesHistory.Last();
				// distance (km)
				distance = GetDistance(lastCoordinates.Latitude, lastCoordinates.Longitude, 
											coordinates.Latitude, coordinates.Longitude);
				//timePeriod (min)
				timePeriod = CountOfMinutes(lastCoordinates.AddedTime, coordinates.AddedTime);
				speed = distance / (timePeriod / 60); // km/h
				coordinatesHistory.Add(coordinates);
				if (speed > 5)
				{
					currentTarif = tarifManager.GetById(coordinates.TarifId);
					return (decimal)(timePeriod * (double)currentTarif.OneMinuteCost);
				}
				else
				{
					currentTarif = tarifManager.GetById(coordinates.TarifId);
					return (decimal)(timePeriod * (double)currentTarif.WaitingCost);
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
		}
	}
}
