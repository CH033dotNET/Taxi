using BAL.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model.DTO;

namespace TaxiAPI.Controllers
{
	public class UserController : ApiController
	{
		private IPersonManager personManager;

		public UserController(IPersonManager personManager)
		{
			this.personManager = personManager;
		}
	}
}
