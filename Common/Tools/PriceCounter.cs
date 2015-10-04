using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools
{
	public class PriceCounter
	{
		public float MinPrice { get; set; }
		public float StartPrice { get; set; }
		public float OneMinuteCost { get; set; }
		public float WaitingCost { get; set; }
		private float FinalPrice { get; set; }

		private DateTime StartTime;
		private bool isWaitingTime = false;

		public PriceCounter(float minPrice, float startPrice, float oneMinuteCost, float waitingCost)
		{
			MinPrice = minPrice;
			StartPrice = startPrice;
			OneMinuteCost = oneMinuteCost;
			WaitingCost = waitingCost;
		}

		public void StartCounter()
		{
			StartTime = DateTime.Now;
		}

		public float StopCounter()
		{
			if (isWaitingTime)
			{
				FinalPrice += StartPrice + (OneMinuteCost * CountOfMinutes(StartTime, DateTime.Now));
			}
			else
			{
				FinalPrice += StartPrice + (WaitingCost * CountOfMinutes(StartTime, DateTime.Now));
			}

			if (FinalPrice < MinPrice)
			{
				return MinPrice;
			}
			else
			{
				return FinalPrice;
			}
		}

		public void EnableWaitingTime()
		{
			isWaitingTime = true;
		}

		public void DizableWaitingTime()
		{
			isWaitingTime = false;
		}

		public float CountOfMinutes(DateTime start, DateTime end)
		{

			return (float)((end.Millisecond - start.Millisecond)/60000);
		}
	}
}
