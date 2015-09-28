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
		CarManager carManager;
		public DriverController()
		{
			//Nick
			carManager = new CarManager(base.uOW);
		}
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DistrictPart()
		{
			int? userId = null;
			int? userRoleId = null;
			if (Session["User"] != null)
			{
				userId = ((UserDTO)Session["User"]).Id;
				userRoleId = ((UserDTO)Session["User"]).RoleId;
				if (userRoleId != 1)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
			}
			return PartialView(carManager.GetWorkingDrivers());
		}
    }
}
