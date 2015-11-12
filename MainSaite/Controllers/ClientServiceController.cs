using BAL.Manager;
using Common.Tools;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MainSaite.Helpers;
using BAL.Interfaces;

namespace MainSaite.Controllers
{
	public class ClientServiceController : BaseController
	{

		private ITarifManager tarifManager;
		private IOrderManager orderManager;
		private ICoordinatesManager coordinatesManager;
		private IDriverLocationHelper driverLocationHelper;
        public IWorkerStatusManager workerStatusManager;
		public ClientServiceController(ITarifManager tarifManager, IOrderManager orderManager, ICoordinatesManager coordinatesManager, IDriverLocationHelper driverLocationHelper,IWorkerStatusManager workerStatusManager)
		{
			this.orderManager = orderManager;
			this.tarifManager = tarifManager;
			this.coordinatesManager = coordinatesManager;
			this.driverLocationHelper = driverLocationHelper;
			this.coordinatesManager.addedCoords += this.driverLocationHelper.addedLocation;
            this.workerStatusManager = workerStatusManager;
		}

		public ActionResult PeekClient()
		{
			return PartialView(tarifManager.GetTarifes());
		}


		public void StartTrip(CoordinatesDTO coordinates)
		{
			OrderDTO order = new OrderDTO();
			order = FillOrder(coordinates);
			orderManager.InsertOrder(order);
			//TODO: getOrder from script
			order = orderManager.GetNotStartOrderByDriver(SessionUser.Id);
			order.StartWork = DateTime.Now;
			orderManager.EditOrder(order);

            workerStatusManager.ChangeWorkerStatus(SessionUser.Id, "2");
            driverLocationHelper.removeDriverFromUserPage(SessionUser.Id);

		}

		public string DrivingClient(CoordinatesDTO coordinates)
		{
			List<CoordinatesDTO> list = SessionCordinates;
			list.Add(coordinates);
			SessionCordinates = list;
			coordinatesManager.AddCoordinates(coordinates);
			return ShowCurrentPrice();
		}

		private string ShowCurrentPrice()
		{
			List<TarifDTO> tarifs = tarifManager.GetTarifes().ToList();
			PriceCounter price = new PriceCounter(SessionCordinates, tarifs);
			return String.Format("{0:0.00}", price.CalcPrice());
		}
		public string DropClient(CoordinatesDTO coordinates)
		{
			OrderDTO startedOrder = orderManager.GetStartedOrderByDriver(SessionUser.Id);
            workerStatusManager.ChangeWorkerStatus(SessionUser.Id, "0");
            driverLocationHelper.addOnUserPageDriver(SessionUser.Id, coordinates.Latitude, coordinates.Longitude, coordinates.AddedTime, "Free");
			if (startedOrder != null)
			{
				startedOrder.EndWork = DateTime.Now;
				orderManager.EditOrder(startedOrder);
			}
			coordinatesManager.AddCoordinates(coordinates);
			string price = ShowCurrentPrice();
			SessionCordinates = new List<CoordinatesDTO>();
			decimal minPrice = tarifManager.GetById(coordinates.TarifId).MinimalPrice;
			if (minPrice > Decimal.Parse(price))
			{
				return String.Format("{0:0.00}", minPrice);
			}
			else
			{
				return price;
			}
          			
		}

		public bool IsInTheWay()
		{
			OrderDTO nonStartedTrip = orderManager.GetNotStartOrderByDriver(SessionUser.Id);
			if (nonStartedTrip != null)
			{
				return false;
			}
			else
			{
				OrderDTO StartedTrip = orderManager.GetStartedOrderByDriver(SessionUser.Id);
				if (StartedTrip != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		private OrderDTO FillOrder(CoordinatesDTO coordinates)
		{
			OrderDTO order = new OrderDTO();
			order.Accuracy = 30.7F;
			order.PeekPlace = "Start";
			order.DropPlace = "Finish";
			order.DriverId = coordinates.UserId;
			order.LatitudePeekPlace = 10.3F;
			order.LatitudeDropPlace = 10.3F;
			order.LongitudePeekPlace = 10.3F;
			order.LongitudeDropPlace = 10.3F;
			order.PersonId = 7;
			order.TotalPrice = 43;
			order.RunTime = "20";
			order.OrderTime = DateTime.Now;
			//order.StartWork = DateTime.Parse("10/10/2000 12:00:00");
			//order.EndWork = DateTime.Parse("10/10/2000 12:00:00");
			return order;
		}

        public JsonResult CheckConnection()
        {
            return Json(new { data = "OK" });
        }

        public JsonResult OrderResult(CoordinatesDTO[][] Trips, string Price)
        {
            OrderDTO startedOrder = orderManager.GetStartedOrderByDriver(Trips[0].First().UserId);

            //TEMP //orderManager.EditOrder(startedOrder, Trips[0].First().AddedTime, Trips[0].Last().AddedTime, Price);
            coordinatesManager.AddRangeCoordinates(Trips[0].ToList(), startedOrder.Id);

            SessionCordinates = new List<CoordinatesDTO>();
            return Json(new { data = "OK" });
        }

	}

}
