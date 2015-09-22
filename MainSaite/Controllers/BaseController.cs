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

		public BaseController()
		{
			uOW = new UnitOfWork();
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
			}

			base.OnActionExecuting(filterContext);
		}

    }
}
