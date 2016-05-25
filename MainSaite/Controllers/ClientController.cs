﻿using BAL.Interfaces;
using BAL.Manager;
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
		private IFeedbackManager feedbackManager;
		private IUserManager userManager;

		private static IHubContext Context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

		public ClientController(IOrderManagerEx orderManager, IPersonManager personManager, IFeedbackManager feedbackManager, IUserManager userManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
			this.feedbackManager = feedbackManager;
			this.userManager = userManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult AddOrder(OrderExDTO order)
		{
            return Json(orderManager.AddOrder(order));
		}

		[HttpPost]
		public JsonResult GetOrder(int id)
		{
			return Json(orderManager.GetById(id));
		}

		[HttpPost]
		public JsonResult GetPerson(int id)
		{
			return Json(personManager.GetPersonByUserId(id));
		}

		public ActionResult DriveHistory()
		{
			return View(orderManager.GetOrdersByUserId(SessionUser.Id));
		}
		public ActionResult ClientBonuses()
		{
			return View(userManager.GetById((Session["User"] as UserDTO).Id).Bonus);
		}
		[HttpPost]
		public JsonResult SetClientBonus(int userId, decimal bonus)
		{
			
			return Json(true);
		}
		public JsonResult UpdateOrder(OrderExDTO order)
		{
			orderManager.UpdateOrder(order);
			return Json(orderManager.GetById(order.Id));
		}

		[HttpPost]
		public JsonResult GetFeedback(int id)
		{
			return Json(feedbackManager.GetById(id));
		}

		[HttpPost]
		public JsonResult UpdateFeedback(FeedbackDTO feedback)
		{
			return Json(feedbackManager.UpdateFeedback(feedback));
		}

		[HttpPost]
		public JsonResult AddFeedback(FeedbackDTO feedback)
		{
			return Json(feedbackManager.AddFeedback(feedback));
		}

		[HttpPost]
		public void SetClientFeedback(int orderId, int feedbackId)
		{
			orderManager.SetClientFeedback(orderId, feedbackId);
		}

		[HttpPost]
		public void CancelOrder(int id)
		{
			orderManager.CancelOrder(id);
		}
	}
}
