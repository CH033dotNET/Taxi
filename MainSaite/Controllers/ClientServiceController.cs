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
		// GET: /ClientService/

		public ActionResult PeekClient()
		{
			return PartialView(tarifManager.GetTarifes());
		}

		public string DrivingClient(CoordinatesDTO coordinates)
		{
			List<CoordinatesDTO> list = session.Coordinates;
			list.Add(coordinates);
			session.Coordinates = list;
			coordinatesManager.AddCoordinates(coordinates);
			return ShowCurrentPrice();
		}

		public string ShowCurrentPrice()
		{
			List<TarifDTO> tarifs = tarifManager.GetTarifes().ToList();
			PriceCounter price = new PriceCounter(session.Coordinates, tarifs);
			return String.Format("{0:0.00}", price.CalcPrice());
		}
		public string DropClient(CoordinatesDTO coordinates)
		{
			coordinatesManager.AddCoordinates(coordinates);
			string temp = ShowCurrentPrice();
			session.Coordinates = new List<CoordinatesDTO>();
			return temp;
		}

	}
}
