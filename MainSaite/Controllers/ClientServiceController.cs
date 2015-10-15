using Common.Tools;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class ClientServiceController : BaseController
	{
		private static PriceCounter priceCounter;
		//
		// GET: /ClientService/

		public ClientServiceController()
		{
			if(priceCounter == null)
				priceCounter = new PriceCounter(tarifManager);
		}

		public ActionResult PeekClient()
		{
			return PartialView(tarifManager.GetTarifes());
		}

		public string DrivingClient(CoordinatesDTO coordinates)
		{			
			return String.Format("{0:0.00}", priceCounter.CounterTick(coordinates));
		}

		public string DropClient(CoordinatesDTO coordinates)
		{
			decimal finallPrice = priceCounter.CounterTick(coordinates);
			priceCounter.StopCounter();
			return String.Format("{0:0.00}", finallPrice);
		}

	}
}
