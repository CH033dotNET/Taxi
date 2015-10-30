using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;
using Model.DTO;
using Common.Helpers;
using Common.Enum;

namespace TaxiAPI.Controllers
{
    public class AddressController : ApiController
    {
		private IAddressManager addressmanager;
		public AddressController(IAddressManager addressmanager)
		{
			this.addressmanager = addressmanager;
		}
    }
}
