using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MainSaite.Models;
using Model.DTO;

namespace TaxiAPI.Controllers
{
	public class UserPageController : ApiController
	{
		private IAddressManager addressmanager;
		private IPersonManager personManager;
		private IOrderManager orderManager;
		public UserPageController(IAddressManager addressmanager, IPersonManager personManager, IOrderManager orderManager)
		{
			this.addressmanager = addressmanager;
			this.personManager = personManager;
			this.orderManager = orderManager;
		}
	}
}
