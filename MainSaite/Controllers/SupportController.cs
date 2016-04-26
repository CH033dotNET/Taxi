﻿using BAL.Interfaces;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class SupportController : BaseController
	{
		private ISupportManager SupportManager;

		public SupportController(ISupportManager supportManager)
		{
			SupportManager = supportManager;
		}

		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetMessages()
		{
			return Json(SupportManager.GetMessages(SessionUser.Id), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public void SendMessage(string message, int toUserID)
		{
			SupportManager.SendMessage(message, SessionUser.Id, toUserID);
		}

		[HttpPost]
		public JsonResult GetSupporter()
		{
			return Json(SupportManager.GetSupporter(), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult GetChatUsers()
		{
			return Json(SupportManager.GetChatUsers(), JsonRequestBehavior.AllowGet);
		}
	}
}