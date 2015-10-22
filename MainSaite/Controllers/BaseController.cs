using BAL.Manager;
using DAL;
using MainSaite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
	public class BaseController : Controller
	{

		//
		// GET: /Base/
		protected UnitOfWork uOW;
		protected UserManager userManager;
		protected AddressManager addressmanager;
		protected CarManager carManager;
		protected DistrictManager districtManager;
		protected PersonManager personManager;
		protected LocationManager locationManager;
		protected TarifManager tarifManager;
		protected LocationManager locationmanager;
		protected DistrictManager districtmanager;
		protected CoordinatesManager coordinatesManager;
		protected OrderManager orderManager;

		public UserDTO SessionUser
		{
			get
			{
				return Session["User"] as UserDTO;
			}
			set
			{
				Session["User"] = value;
			}
		}
		public List<CoordinatesDTO> SessionCordinates
		{
			get
			{
				return Session["Coordinates"] as List<CoordinatesDTO>;
			}
			set
			{
				Session["Coordinates"] = value;
			}
		}

		public BaseController()
		{
			uOW = new UnitOfWork();
			addressmanager = new AddressManager(uOW);
			userManager = new UserManager(uOW);
			carManager = new CarManager(uOW);
			districtManager = new DistrictManager(uOW);
			personManager = new PersonManager(uOW);
			locationManager = new LocationManager(uOW);
			tarifManager = new TarifManager(uOW);
			locationmanager = new LocationManager(uOW);
			districtmanager = new DistrictManager(uOW);
			coordinatesManager = new CoordinatesManager(uOW);
			orderManager = new OrderManager(uOW);
            coordinatesManager.addedCoords += driversLocationHub.addedLocation;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (Session["Culture"] != null)
			{
				CultureInfo cultInfo = new CultureInfo((string)Session["Culture"]);
				Thread.CurrentThread.CurrentUICulture = cultInfo;
			}



			if (SessionUser != null)
			{
				UserDTO user = SessionUser;
				ViewBag.UserRoleId = user.Role.Id;
				ViewBag.ImageName = personManager.GetPersonByUserId(user.Id).ImageName;
			}

			if (SessionCordinates == null)
			{
				SessionCordinates = new List<CoordinatesDTO>();
			}

			base.OnActionExecuting(filterContext);
		}

	}
}
