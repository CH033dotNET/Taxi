using BAL.Manager;
using Common.Enum;
using DAL;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MainSaite.Controllers
{
	public class SettingsController : BaseController
	{
		UserManager userManager;

		//ASIX
		DistrictManager districtManager;

		// Nick
		CarManager carManager;
		


		public SettingsController()
		{
			userManager = new UserManager(base.uOW);

			//ASIX
			districtManager = new DistrictManager(base.uOW);

			//Nick
			carManager = new CarManager(base.uOW);
		}


		public ActionResult Index()
		{
			return View();
		}


		public ActionResult UsersMenu()
		{
			var users = userManager.GetUsers();

			return View(users);
		}

		public ActionResult ChangeMenu(int id = 0)
		{

			var user = userManager.GetById(id);
			if (user == null)
			{
				return HttpNotFound();
			}

			return View(user);
		}
		[HttpPost]
		public ActionResult ChangeMenu(UserDTO user)
		{
			if (ModelState.IsValid)
			{
				///Think about this 3 strings
				userManager.ChangeUserParameters(user);
				return RedirectToAction("UsersMenu");
			}

			return View(user);
		}

		public ActionResult DistrictEditor()
		{
			return View("DistrictEditor", districtManager.getDistricts());
		}

		[HttpPost]
		public ActionResult DistrictEditor(string Name)
		{
				districtManager.addDistrict(Name);
			return RedirectToAction("DistrictEditor");
		}
		
		public ActionResult DeleteDistrict(District a)
		{
			districtManager.deleteById(a.Id);
			return RedirectToAction("DistrictEditor");
		}

		// Nick: Car info settings
	
		public ActionResult CarEditor()
		{
		    int? userId = null;
			if (Session["User"]!=null)
			{
				userId = ((UserDTO)Session["User"]).Id; 
			}
			return View(carManager.getCarsByUserID(userId));
		}
		// GET:
		public ActionResult CarCreate()
		{
			return View();
		}

		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarCreate(CarDTO car)
		{
			try
			{
				if (ModelState.IsValid)
				{
					carManager.addCar(car);
					return RedirectToAction("CarEditor");
				}
			}
			catch (DataException)
			{
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("CarEditor");
		}

		public ActionResult CarDetails(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carID = carManager.GetCarByCarID(id);
			if (carID == null)
			{
				return HttpNotFound();
			}
			return View(carID);
		}
		// GET: 
		public ActionResult CarDelete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carForDelete = carManager.GetCarByCarID(id);
			if (carForDelete == null)
			{
				return HttpNotFound();
			}
			return View(carForDelete);
		}
		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarDelete(CarDTO car)
		{
			try
			{
				carManager.deleteCarByID(car.Id);
			}
			catch (DataException)
			{
				return RedirectToAction("CarDelete");
			}
			return RedirectToAction("CarEditor");
		}
		// GET: 
		public ActionResult CarEdit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var carID = carManager.GetCarByCarID(id);
			if (carID == null)
			{
				return HttpNotFound();
			}
			return View(carID);
		}

		// POST:
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CarEdit(CarDTO car)
		{
			if (ModelState.IsValid)
			{
				carManager.EditCar(car);
				return RedirectToAction("CarEditor");
			}
			return RedirectToAction("CarEditor");
		}

		public ActionResult SetVIPStatus()
		{
			List<Object> ListForView = new List<object>() { userManager.GetVIPClients(), userManager.GetNoVIPClients() };
			return View(ListForView);
		}

		[HttpPost]
		public ActionResult SetVIPStatusC(string UserName)
		{
			if (UserName != null)
			{
				userManager.SetVIPStatus(UserName);
			}
			return RedirectToAction("SetVIPStatus");
		}

    }
}
