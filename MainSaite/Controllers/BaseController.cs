using BAL.Manager;
using DAL;
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


		public BaseController()
		{
			uOW = new UnitOfWork();
			userManager = new UserManager(uOW);
			carManager = new CarManager(uOW);
			districtManager = new DistrictManager(uOW);
			personManager = new PersonManager(uOW);
			locationManager = new LocationManager(uOW);
            tarifManager = new TarifManager(uOW);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (Session["Culture"] != null)
			{
				CultureInfo cultInfo = new CultureInfo((string)Session["Culture"]);
				Thread.CurrentThread.CurrentUICulture = cultInfo;
			}


			
			if(!(Session["User"] == null))
			{ 
				UserDTO user = Session["User"] as UserDTO;
				ViewBag.UserRoleId = user.Role.Id;
				ViewBag.ImageName = personManager.GetPersonByUserId(user.Id).ImageName;
			}

			base.OnActionExecuting(filterContext);
		}

    }
}
