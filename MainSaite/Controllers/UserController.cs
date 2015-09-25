using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Manager;
using Model.DB;
using Model.DTO;

namespace MainSaite.Controllers
{
	public class UserController : BaseController
	{

	
		PersonManager personManager;
		//UserDTO currentUser;
	
		
		public UserController()
		{
		
		    personManager= new PersonManager(uOW);
		}

		public ActionResult Index()
		{

			var currentUser = (UserDTO)(Session["User"]);
			
			if (currentUser == null)
				RedirectToAction("Registration", "Account");
			
			var currentPerson = personManager.GetPersonByUserId(currentUser.Id);
			if (currentPerson == null)
			currentPerson =	personManager.InsertPerson(new PersonDTO() {UserId = currentUser.Id });
			currentPerson.User = currentUser;
			
			return View(currentPerson);

		}

		[HttpPost]
		public ActionResult Index(PersonDTO person)
		{
			var currentUser = (UserDTO)(Session["User"]);

			if (person.User.UserName != currentUser.UserName)
				currentUser.UserName = person.User.UserName;
			
			if (person.User.Email != currentUser.Email)
				currentUser.Email = person.User.Email;

			person.UserId = currentUser.Id;
			person.User = currentUser;
			personManager.EditPerson(person);

			Session["User"] = currentUser;
				
			return View(person);
			
		}
	}
}
