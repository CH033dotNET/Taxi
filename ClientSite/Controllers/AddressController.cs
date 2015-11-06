using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class AddressController : BaseController
    {
        //
        // GET: /Address/
        public ActionResult Index()
        {
			//var addressList = addressmanager.GetAddressesForUser(SessionUser.Id);
			//return View(addressList);
			return View();
        }
	}
}