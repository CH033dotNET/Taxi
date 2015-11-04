using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DB;
using BAL.Manager;
using Model.DTO;

namespace MainSaite.Controllers
{
    public class OrderController : BaseController
    {
		private IOrderManager orderManager;
		private IPersonManager personManager;

		public OrderController(IOrderManager orderManager, IPersonManager personManager)
		{
			this.orderManager = orderManager;
			this.personManager = personManager;

		}

		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetOrder(OrderDTO order)
		{

			order.PersonId = SessionPerson.Id;
			orderManager.InsertOrder(order);
			return Json(JsonRequestBehavior.AllowGet);
		}


		public JsonResult OrdersData()
		{
			var orders = orderManager.GetOrders().ToList();
			var peoples = personManager.GetPersons().ToList();

			var operatorOrders = from O in orders
								 join P in peoples
								 on O.PersonId equals P.Id
								 select new { FirsName = P.FirstName, OrderTime = O.OrderTime, PeekPlace = O.PeekPlace, DropPlace = O.DropPlace };

			return Json(operatorOrders, JsonRequestBehavior.AllowGet);
		}

    }
}

