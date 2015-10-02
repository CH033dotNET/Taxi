using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Models
{
	public class WebViewPageBase : WebViewPage
	{
		public new UserDTO User
		{
			get
			{				
					return HttpContext.Current.Session["User"] as UserDTO;
			}
			set
			{
				HttpContext.Current.Session["User"] = value;
			}
		}

		public override void InitHelpers()
		{
			base.InitHelpers();

		}

		public override void Execute()
		{

		}
	}

	public class WebViewPageBase<T> : WebViewPage<T>
	{
		public new UserDTO User
		{
			get
			{			
				return HttpContext.Current.Session["User"] as UserDTO;
			}
			set
			{
				HttpContext.Current.Session["User"] = value;
			}
		}

		public override void InitHelpers()
		{
			base.InitHelpers();

		}

		public override void Execute()
		{

		}

	}
}