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

		private static RequestHelper requestHelper;

		/// <summary>
		/// Provides url for connection to MainSite. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper MainSiteRequestHelper
		{
			get
			{
				if (requestHelper == null)
				{
					requestHelper = new RequestHelper(WebConfigurationManager.AppSettings["MainSiteUrl"]);
				}
				return requestHelper;
			}
		}
		/// <summary>
		/// Provides url for connection to Phone App. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper PhoneRequestHelper
		{
			get
			{
				if (requestHelper == null)
				{
					requestHelper = new RequestHelper(WebConfigurationManager.AppSettings["PhoneAppUrl"]);
				}
				return requestHelper;
			}
		}
		/// <summary>
		/// Provides url for connection to Tablet App. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper TabletRequestHelper
		{
			get
			{
				if (requestHelper == null)
				{
					requestHelper = new RequestHelper(WebConfigurationManager.AppSettings["TabletAppUrl"]);
				}
				return requestHelper;
			}
		}

    }
}
