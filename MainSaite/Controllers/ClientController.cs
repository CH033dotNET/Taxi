﻿using BAL.Interfaces;
using MainSaite.Hubs;
using Microsoft.AspNet.SignalR;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class ClientController : BaseController
	{
		private IOrderManagerEx orderManager;
		private IPersonManager personManager;

		private static IHubContext Context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();

		public ClientController(IOrderManagerEx orderManager, IPersonManager personManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult AddOrder(OrderExDTO order)
		{
			var newOrder = orderManager.AddOrder(order);
			return Json(newOrder);
        }

		public ActionResult OrderForm()
		{
			return View();
		}

		[HttpPost]
		public JsonResult GetPerson(int id)
		{
			return Json(personManager.GetPersonByUserId(id));
		}
	}
}
