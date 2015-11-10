using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			var addressList = ApiRequestHelper.Get<List<AddressDTO>, int>("Address", "GetAddressesForUser", SessionUser.Id);
			if (addressList.StatusCode == HttpStatusCode.OK)
			{
				return View(addressList.Data);
			}

			return View(addressList.Data);
        }

		[HttpGet]
		public ActionResult CreateAddress()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateAddress(AddressDTO address)
		{
			//addressmanager.AddAddress(address);
			var myAddress = ApiRequestHelper.postData<AddressDTO>("Address", "AddAddress", address).Data as AddressDTO;
			return RedirectToAction("Index");
		}

		[HttpGet]
		public string TestMethod()
		{
			var igo = ApiRequestHelper.Get<int, int>("Address", "GetThat", 4).Data;
			var gget = ApiRequestHelper.postData<int>("Address", "GetPost", 7).Data;

			var igo1 = ApiRequestHelper.Get<int, int>("Account", "TestGet", 4).Data;
			var gget1 = ApiRequestHelper.postData<int>("Account", "TestPost", 5).Data;
			return igo.ToString();
			}

		[HttpGet]
		public ActionResult DeleteAddress(int id)
		{

			//AddressDTO address = addressmanager.GetById(id);
			AddressDTO address = ApiRequestHelper.Get<AddressDTO, int>("Address", "GetById", id).Data as AddressDTO;
			return View(address);
		}
		[HttpPost, ActionName("DeleteAddress")]
		public ActionResult DeleteAdd(int id)
		{
			//addressmanager.DeleteAddress(id);
			var delete = ApiRequestHelper.Get<int, int>("Address", "DeleteAddress", id).Data;
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult EditAddress(int id)
		{

			//AddressDTO address = addressmanager.GetById(id);
			var address = ApiRequestHelper.Get<AddressDTO, int>("Address", "GetById", id).Data as AddressDTO;
			return View(address);
		}
		[HttpPost]
		public ActionResult EditAddress(AddressDTO address)
		{
			//addressmanager.UpdateAddress(address);
			var myAddress = ApiRequestHelper.postData<AddressDTO>("Address", "UpdateAddress", address);

			return RedirectToAction("Index");
		}
	}
}