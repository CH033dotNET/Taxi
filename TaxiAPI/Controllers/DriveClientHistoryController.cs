using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.Manager;
using Model.DTO;

namespace TaxiAPI.Controllers
{
    public class DriveClientHistoryController : ApiController
    {
		private IOrderManager orderManager;
		public DriveClientHistoryController(IOrderManager orderManager)
		{
			this.orderManager = orderManager;
		}
    }
}
