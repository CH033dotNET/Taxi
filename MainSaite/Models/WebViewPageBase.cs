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
		}
        public List<TarifDTO> Tarifes
        {
            get
            {
                return HttpContext.Current.Session["Tarifes"] as List<TarifDTO>;
            }
        }

		public List<CoordinatesDTO> Coordinates
		{
			get
			{
				return HttpContext.Current.Session["Coordinates"] as List<CoordinatesDTO>;
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
		}

        public List<TarifDTO> Tarifes
        {
            get
            {
                return HttpContext.Current.Session["Tarifes"] as List<TarifDTO>;
            }
        }

		public List<CoordinatesDTO> Coordinates
		{
			get
			{
				return HttpContext.Current.Session["Coordinates"] as List<CoordinatesDTO>;
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