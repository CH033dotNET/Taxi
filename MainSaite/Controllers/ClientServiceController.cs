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

		//
		// GET: /ClientService/

		public ActionResult PeekClient()
		{
			return PartialView(tarifManager.GetTarifes());
		}

		public ActionResult DrivingClient(CoordinatesDTO coordinates)
		{
               /*current Price*/

			return PartialView(coordinates.TarifId);
		}

		public ActionResult DropClient(CoordinatesDTO coordinates)
		{

			return PartialView(coordinates.TarifId = 100);
		}

	}
}
