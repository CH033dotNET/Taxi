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
				RedirectToAction("Registration", "Account");

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

			UploudImage(person, formCollection, currentUser);


			person.UserId = currentUser.Id;
			person.User = currentUser;

			personManager.EditPerson(person);
			Session["User"] = currentUser;
			ViewBag.ImageName = person.ImageName;




			return View(person);

		}

		public void UploudImage(PersonDTO person, FormCollection formCollection, UserDTO currentUser)
		{

			var profileAvatar = "item_0_profile.jpg";

			person.ImageName = personManager.GetPersonByUserId(currentUser.Id).ImageName;

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
					string mapImage = Server.MapPath(@"~\Images\") + person.ImageName;

					//delete last image
					if (file.FileName != person.ImageName)
						imageUpload.DeleteFile(person.ImageName, profileAvatar, mapImage);

					// rename, resize, and upload
					//return object that contains {bool Success,string ErrorMessage,string ImageName}
					ImageResult imageResult = imageUpload.RenameUploadFile(file);
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
		}

		
		
		}
	}
