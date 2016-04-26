using ClientSite.Models;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class UserPageController : BaseController
    {
		public ActionResult Index()
		{
			UserPagePhoneModel model = new UserPagePhoneModel();

			if (SessionUser != null)
			{
				var addresses = ApiRequestHelper.Get<List<AddressDTO>, int>("UserPage", "GetAddressesForUser", SessionUser.Id).Data as List<AddressDTO>;
				model.addresses = addresses.ToList();
				model.person = ApiRequestHelper.Get<PersonDTO, int>("UserPage", "GetPersonByUserId", SessionUser.Id).Data as PersonDTO;
				var droPlaces = ApiRequestHelper.Get<List<OrderDTO>, int>("UserPage", "GetOrdersByUserId", SessionUser.Id).Data as List<OrderDTO>;
				model.DroPlaces = droPlaces.Select(x => x.DropPlace).ToList();
			}
			return View(model);
		}

		[HttpPost]
		public JsonResult Index(PersonDTO person)
		{
			var jsonOk = new { success = true };
			var jsonNeOk = new { success = false, person = person };
			if (!ModelState.IsValid)
				return Json(jsonNeOk);

			var update = ApiRequestHelper.postData<PersonDTO>("UserPage", "UpdatePhoneFMLnames", person).Data as PersonDTO;

			return Json(jsonOk);
		}


		public ActionResult Iframe()
		{
			return View();
		}
	}
}