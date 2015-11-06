using Common.Helpers;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class BaseController : Controller
    {
		private static RequestHelper requestHelper;
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
			if (Session["Culture"] != null)
			{
				CultureInfo cultInfo = new CultureInfo((string)Session["Culture"]);
				Thread.CurrentThread.CurrentUICulture = cultInfo;
			}



			if (SessionUser != null && SessionUser.Email != null)
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