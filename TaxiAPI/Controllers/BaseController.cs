using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace TaxiAPI.Controllers
{
    public class BaseController : ApiController
    {

		private static RequestHelper mainRquestHelper;
		private static RequestHelper driverRquestHelper;
		private static RequestHelper clientRquestHelper;

		/// <summary>
		/// Provides url for connection to MainSite. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper MainSiteRequestHelper
		{
			get
			{
				//if (mainRquestHelper == null)
				//{
					mainRquestHelper = new RequestHelper(WebConfigurationManager.AppSettings["MainSiteAPIUrl"]);
				//}
				return mainRquestHelper;
			}
		}
		/// <summary>
		/// Provides url for connection to Phone App. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper DriverRequestHelper
		{
			get
			{
				//if (driverRquestHelper == null)
				//{
					driverRquestHelper = new RequestHelper(WebConfigurationManager.AppSettings["DriverAPIUrl"]);
				//}
				return driverRquestHelper;
			}
		}
		/// <summary>
		/// Provides url for connection to Tablet App. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper ClientRequestHelper
		{
			get
			{
				//if (clientRquestHelper == null)
				//{
					clientRquestHelper = new RequestHelper(WebConfigurationManager.AppSettings["ClientAPIUrl"]);
				//}
				return clientRquestHelper;
			}
		}

    }
}
