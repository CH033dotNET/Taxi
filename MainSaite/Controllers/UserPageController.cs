using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class UserPageController : BaseController
    {
        //
        // GET: /UserPage/

        public ActionResult Index()
        {
			List<AddressDTO> list = new List<AddressDTO>();
			if (Session!=null && session.User!=null)
			list = addressmanager.GetAddressesEmulation().Where(x=>x.UserId==session.User.Id).ToList();

			return View(list);
        }

    }
}
