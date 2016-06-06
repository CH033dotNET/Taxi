using BAL.Interfaces;
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
		private ITariffExManager tariffManager;

		private static IHubContext Context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();

		public ClientController(
			IOrderManagerEx orderManager,
			IPersonManager personManager,
			IFeedbackManager feedbackManager,
			IUserManager userManager,
			ITariffExManager tariffManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;
			this.feedbackManager = feedbackManager;
			this.userManager = userManager;
			this.tariffManager = tariffManager;
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


		//logic for bonuses
		public ActionResult ClientBonuses()
		{
			var bonus = userManager.GetById((Session["User"] as UserDTO).Id).Bonus;
			return View(Math.Round(bonus,1));
		}
		[HttpPost]
		public JsonResult SetClientBonus(int userId, double bonus, double paidByBonus)
		{
			userManager.SetClientBonus(userId, bonus, paidByBonus);
			return Json(true);
		}
		[HttpPost]
		public JsonResult UseClientBonus(int userId, double price)
		{
			var userBonus = userManager.GetById(userId).Bonus;

			double newPrice = 0.0f;

			if (userBonus>=price) {
				newPrice = 0.0f;
			}
			else newPrice = price - userBonus;

			return Json(newPrice, JsonRequestBehavior.AllowGet);
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
			var newFeedback = feedbackManager.UpdateFeedback(feedback);
            userManager.CalculateUserRating(newFeedback.Id);
			return Json(newFeedback);
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
			var order = orderManager.GetById(orderId);
			feedbackManager.SetUserId(feedbackId, order.DriverId);
			userManager.CalculateUserRating(feedbackId);
		}

		[HttpPost]
		public void CancelOrder(int id)
		{
			orderManager.CancelOrder(id);
		}

		[HttpPost]
		public JsonResult GetTariff()
		{
			return Json(tariffManager.GetStandardTariff());
		}
	}
}
