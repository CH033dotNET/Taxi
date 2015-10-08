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
		private List<CoordinatesDTO> coordinatesHistory = new List<CoordinatesDTO>();
		private decimal finalPrice = 0;
		private decimal currentPrice = 0;
		private const double WAITINGCOSTSPEED = 5;

		public PriceCounter(CoordinatesDTO coordinates)
		{
			coordinatesHistory.Add(coordinates);
		}

		public decimal CounterTick(CoordinatesDTO coordinates)
		{
			//Here must be calculating 
			return 1;
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
