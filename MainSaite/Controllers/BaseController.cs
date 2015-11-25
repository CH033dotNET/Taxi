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
using MainSaite.Helpers;
using Common.Helpers;
using System.Web.Configuration;

namespace MainSaite.Controllers
{
	public class BaseController : Controller
	{
		private static RequestHelper requestHelper;

		/// <summary>
		/// Provides url for connection to WebApi. "______Url" refers to entry in library web.config file.
		/// </summary>
		public RequestHelper ApiRequestHelper
		{
			get
			{
				if (requestHelper == null)
				{
					requestHelper = new RequestHelper(WebConfigurationManager.AppSettings["WebAPIUrl"]);
				}
				return requestHelper;
			}
		}


		public PersonDTO SessionPerson
		{
			get
			{
				return Session["Person"] as PersonDTO;
			}
			set
			{
				Session["Person"] = value;
			}
		}

        public List<TarifDTO> Tarifes
        {
            get
            {
                return Session["Tarifes"] as List<TarifDTO>;
            }
            set
            {
                Session["Tarifes"] = value;
            }

        }

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

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
            CultureInfo cultInfo =  new CultureInfo("en-us");

            if (SessionUser != null)
            {
                cultInfo = new CultureInfo(SessionUser.Lang);
                Session["Culture"] = SessionUser.Lang;
            }
            else if (Session["Culture"] != null)
            {
                cultInfo = new CultureInfo((string)Session["Culture"]);
               
            }
            Thread.CurrentThread.CurrentUICulture = cultInfo;



			if (SessionUser != null)
			{
				UserDTO user = SessionUser;
				ViewBag.UserRoleId = user.Role.Id;
				ViewBag.ImageName = SessionPerson.ImageName;
			}

			if (SessionCordinates == null)
			{
				SessionCordinates = new List<CoordinatesDTO>();
			}

			base.OnActionExecuting(filterContext);
		}

	}
}
