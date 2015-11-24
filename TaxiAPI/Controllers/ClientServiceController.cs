using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;
using Common.Tools;
using BAL.Manager;

namespace TaxiAPI.Controllers
{
    public class ClientServiceController : BaseController
    {
		private ITarifManager tarifManager;
		private IOrderManager orderManager;
		private ICoordinatesManager coordinatesManager;
		public ClientServiceController(ITarifManager tarifManager, IOrderManager orderManager, ICoordinatesManager coordinatesManager)
		{
			this.orderManager = orderManager;
			this.tarifManager = tarifManager;
			this.coordinatesManager = coordinatesManager;
			this.coordinatesManager.addedCoords += coordinates => MainSiteRequestHelper.postData<bool, CoordinatesDTO>("OperatorHub", "AddedLocation", coordinates);
		}
        [HttpGet]
        [Route("api/ClientService/GetTarifes")]
        public HttpResponseMessage GetTarifes()
        {
            return Request.CreateResponse(HttpStatusCode.OK, tarifManager.GetTarifes());
        }
        [HttpGet]
        [Route("api/ClientService/GetNotStartOrderByDriver")]
        public HttpResponseMessage GetNotStartOrderByDriver(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetNotStartOrderByDriver(id));
        }
        [HttpGet]
        [Route("api/ClientService/GetStartedOrderByDriver")]
        public HttpResponseMessage GetStartedOrderByDriver(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetStartedOrderByDriver(id));
        }
        [HttpPost]
        [Route("api/ClientService/InsertOrder")]
        public HttpResponseMessage InsertOrder(OrderDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.InsertOrder(data));
        }
        [HttpPost]
        [Route("api/ClientService/EditOrder")]
        public HttpResponseMessage EditOrder(OrderDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, orderManager.EditOrder(data));
        }
        [HttpPost]
        [Route("api/ClientService/AddCoordinates")]
        public HttpResponseMessage AddCoordinates(CoordinatesDTO data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, coordinatesManager.AddCoordinates(data));
        }
		[HttpPost]
		[Route("api/ClientService/AddRangeCoordinates")]
		public HttpResponseMessage AddRangeCoordinates(List<CoordinatesDTO> data1, int data2)
		{
			return Request.CreateResponse(HttpStatusCode.OK, coordinatesManager.AddRangeCoordinates(data1,data2));
		}
        [HttpGet]
        [Route("api/ClientService/GetById")]
        public HttpResponseMessage GetById(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, tarifManager.GetById(id));
        }
		[HttpGet]
		[Route("api/ClientService/GetFuelConsumption")]
		public HttpResponseMessage GetFuelConsumption(int data)
		{
			return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetFuelConsumption(data));
		}
		[HttpGet]
		[Route("api/ClientService/GetOrderById")]
		public HttpResponseMessage GetOrderById(int data)
		{
			return Request.CreateResponse(HttpStatusCode.OK, orderManager.GetOrderByOrderID(data));
		}
    }
}
