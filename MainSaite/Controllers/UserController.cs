using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using BAL.Manager;
namespace MainSaite.Controllers
{
	public class UserController : BaseController
	{

		private ImageResult imageResult;
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
		public ActionResult Index(PersonDTO person, FormCollection formCollection)
		{
			

			var currentUser = (UserDTO)(Session["User"]);

			if (person.User.UserName != currentUser.UserName)
				currentUser.UserName = person.User.UserName;
			
			if (person.User.Email != currentUser.Email)
				currentUser.Email = person.User.Email;

			foreach (string item in Request.Files)
			{
				HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
				if (file.ContentLength == 0)
					continue;
				if (file.ContentLength > 0)
				{
					// width + height will force size, care for distortion
					//Exmaple: ImageUpload imageUpload = new ImageUpload { Width = 800, Height = 700 };

					// height will increase the width proportionally
					//Example: ImageUpload imageUpload = new ImageUpload { Height= 600 };

					// width will increase the height proportionally
					ImageUpload imageUpload = new ImageUpload { Width = 200 };

					// rename, resize, and upload
					//return object that contains {bool Success,string ErrorMessage,string ImageName}
					imageResult = imageUpload.RenameUploadFile(file);
					if (imageResult.Success)
					{
						//TODO: write the filename to the db
						person.ImageName = imageResult.ImageName;
					}
					else
					{
						// use imageResult.ErrorMessage to show the error
						ViewBag.Error = imageResult.ErrorMessage;
					}
				}
			}
			
			person.UserId = currentUser.Id;
			person.User = currentUser;
			personManager.EditPerson(person);

			Session["User"] = currentUser;
			



			return View(person);
			
		}

		
		}
	}
