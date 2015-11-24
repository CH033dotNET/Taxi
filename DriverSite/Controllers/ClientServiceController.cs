﻿using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DriverSite.Tools;

namespace DriverSite.Controllers
{
	public class ClientServiceController : BaseController
	{
        readonly string controller = "ClientService";
		public ActionResult PeekClient()
		{
			return PartialView(ApiRequestHelper.Get<List<TarifDTO>>("ClientService", "GetTarifes").Data);
		}

		public void StartTrip(CoordinatesDTO coordinates)
		{
			OrderDTO order = new OrderDTO();
			order = FillOrder(coordinates);
            ApiRequestHelper.postData<OrderDTO>(controller, "InsertOrder", order);
			//orderManager.InsertOrder(order);
			//TODO: getOrder from script
            order = ApiRequestHelper.GetById<OrderDTO>(controller, "GetNotStartOrderByDriver", SessionUser.Id).Data;
			//order = orderManager.GetNotStartOrderByDriver(SessionUser.Id);
			order.StartWork = DateTime.Now;
            ApiRequestHelper.postData<OrderDTO>(controller, "EditOrder", order);
			//orderManager.EditOrder(order);
		}

		public string DrivingClient(CoordinatesDTO coordinates)
		{
			List<CoordinatesDTO> list = SessionCordinates;
			list.Add(coordinates);
			SessionCordinates = list;
            ApiRequestHelper.postData<CoordinatesDTO>(controller, "AddCoordinates", coordinates);
			//coordinatesManager.AddCoordinates(coordinates);
			return ShowCurrentPrice(false);
		}

		private string ShowCurrentPrice(bool lastCalc)
		{
			List<TarifDTO> tarifs = ApiRequestHelper.Get<List<TarifDTO>>("ClientService", "GetTarifes").Data;
			PriceCounter price = new PriceCounter(SessionCordinates, tarifs);
			decimal currentPrice = price.CalcPrice();
			if (lastCalc)
			{
				float consumption = ApiRequestHelper.Get<float, int>("ClientService", "GetFuelConsumption", SessionUser.Id).Data;
				float distance = (float)price.finalDistance;
				OrderDTO order = ApiRequestHelper.Get<OrderDTO, int>("ClientService", "GetOrderById", SessionOrderId).Data;
				order.FuelSpent = (distance * consumption == 0) ? 12F : distance * consumption;
				ApiRequestHelper.postData<OrderDTO>(controller, "EditOrder", order);
			}
			return String.Format("{0:0.00}", currentPrice);
		}
        
		public string DropClient(CoordinatesDTO coordinates)
		{
            OrderDTO startedOrder = ApiRequestHelper.GetById<OrderDTO>(controller, "GetStartedOrderByDriver", SessionUser.Id).Data;
			//OrderDTO startedOrder = orderManager.GetStartedOrderByDriver(SessionUser.Id);
			if (startedOrder != null)
			{
				startedOrder.EndWork = DateTime.Now;
                ApiRequestHelper.postData<OrderDTO>(controller, "EditOrder", startedOrder);
				//orderManager.EditOrder(startedOrder);
			}
            ApiRequestHelper.postData<CoordinatesDTO>(controller, "AddCoordinates", coordinates);
			//coordinatesManager.AddCoordinates(coordinates);
			string price = ShowCurrentPrice(true);
			SessionCordinates = new List<CoordinatesDTO>();
            decimal minPrice = ApiRequestHelper.GetById<TarifDTO>(controller, "GetById", coordinates.TarifId).Data.MinimalPrice;
			//decimal minPrice = tarifManager.GetById(coordinates.TarifId).MinimalPrice;
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
            OrderDTO nonStartedTrip = ApiRequestHelper.GetById<OrderDTO>(controller, "GetNotStartOrderByDriver", SessionUser.Id).Data;
            //OrderDTO nonStartedTrip = orderManager.GetNotStartOrderByDriver(SessionUser.Id);
			if (nonStartedTrip != null)
			{
				return false;
			}
			else
            {
                OrderDTO StartedTrip = ApiRequestHelper.GetById<OrderDTO>(controller, "GetStartedOrderByDriver", SessionUser.Id).Data;
                //OrderDTO StartedTrip = orderManager.GetStartedOrderByDriver(SessionUser.Id);
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
		public JsonResult CheckConnection()
        {
            return Json(new { data = "OK" });
        }

        public JsonResult OrderResult(CoordinatesDTO[][] Trips, string Price)
        {
			OrderDTO startedOrder = ApiRequestHelper.GetById<OrderDTO>(controller, "GetStartedOrderByDriver", Trips[0].First().UserId).Data;
            //OrderDTO startedOrder = orderManager.GetStartedOrderByDriver(Trips[0].First().UserId);
			startedOrder.StartWork = Trips[0].First().AddedTime;
			startedOrder.EndWork = Trips[0].Last().AddedTime;
			startedOrder.TotalPrice = Decimal.Parse(Price);
			ApiRequestHelper.postData<OrderDTO>(controller, "EditOrder", startedOrder);            //orderManager.EditOrder(startedOrder, Trips[0].First().AddedTime, Trips[0].Last().AddedTime, Price);
			ApiRequestHelper.postData<List<CoordinatesDTO>, int>(controller, "AddRangeCoordinates", Trips[0].ToList(), startedOrder.Id);
            //coordinatesManager.AddRangeCoordinates(Trips[0].ToList(), startedOrder.Id);

            SessionCordinates = new List<CoordinatesDTO>();
            return Json(new { data = "OK" });
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
        
	}
}
