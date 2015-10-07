using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools
{
	public class PriceCounter
	{
		public TarifDTO Tarif { get; set; }
		private decimal finalPrice = 0;
		private decimal currentPrice = 0;
		private DateTime previousTime; 
		private const double WAITINGCOSTSPEED = 5;

		public PriceCounter(TarifDTO tarif)
		{
			Tarif = tarif;
		}

		public void StartCounter(DateTime time)
		{
			previousTime = time;
		}

		public decimal CounterTick(double speed, DateTime time)
		{
			if (speed <= WAITINGCOSTSPEED)
			{
				currentPrice += (decimal)((double)Tarif.WaitingCost * CountOfMinutes(previousTime, time));
				finalPrice = Tarif.StartPrice + currentPrice;
				return finalPrice;
			}
			else 
			{
				currentPrice += (decimal)((double)Tarif.OneMinuteCost * CountOfMinutes(previousTime, time));
				finalPrice = Tarif.StartPrice + currentPrice;
				return finalPrice;
			}
		}

		public double CountOfMinutes(DateTime start, DateTime end)
		{

			return (double)((end.Millisecond - start.Millisecond)/60000);
		}

		public void ResetCounter()
		{
			finalPrice = 0;
			currentPrice = 0;
		}
	}
}
