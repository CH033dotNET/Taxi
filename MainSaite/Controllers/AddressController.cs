using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class AddressController : BaseController
    {
		private IAddressManager addressmanager;
		public AddressController(IAddressManager addressmanager)
		{
			this.addressmanager = addressmanager;
		}

        public ActionResult Index()
        {
			return View();
        }

		public JsonResult GetFavoriteAddresses()
		{
			var addresses = addressmanager.GetAddressesForUser(SessionUser.Id);
			return Json(addresses, JsonRequestBehavior.AllowGet);
		}

		public void DelAddress(int addressId)
		{
			addressmanager.DeleteAddress(addressId);
		}

		public void UpdAddress(AddressDTO address)
		{
			addressmanager.UpdAddress(address);
		}


		public void AddAddress(AddressDTO address)
		{
			address.UserId = SessionUser.Id;
			addressmanager.AddAddress(address);
		}
    }
}
