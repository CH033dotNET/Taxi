using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MainSaite.Controllers
{
	public class DriverController : BaseController
	{
		public DriverController()
		{
		}
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DistrictPart(string userAction)
		{
			if (userAction == "WorkStart")
			{
				ViewBag.SubmitValue = "WorkStart";
			}
			if (userAction == "WorkEnd")
			{
				ViewBag.SubmitValue = "WorkEnd";
			}
			return PartialView();
		}
    }
}
