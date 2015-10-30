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
	public class TarifController : ApiController
	{
		private ITarifManager tarifManager;

		public TarifController(ITarifManager tarifManager)
		{
			this.tarifManager = tarifManager;
		}
	}
}
