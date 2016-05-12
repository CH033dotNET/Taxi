using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DTO;
using BAL.Manager;
using BAL.Interfaces;

namespace MainSaite.Controllers
{
	public class UserController : BaseController
	{
		private IPersonManager personManager;
        private IUserManager userManager;

		public UserController(IPersonManager personManager, IUserManager userManager)
		{
			this.personManager = personManager;
            this.userManager = userManager;
		}

		public ActionResult Index()
		{

			var currentUser = SessionUser;

			if (currentUser == null)
				RedirectToAction("Registration", "Account");

			var currentPerson = personManager.GetPersonByUserId(currentUser.Id);

			if (currentPerson == null)
				RedirectToAction("Registration", "Account");

			return View(currentPerson);

		}

		[HttpPost]
		public ActionResult Index(PersonDTO person, FormCollection formCollection, string language)
		{
			var currentUser = SessionUser;
            SessionUser.Lang = language;


			if (person.User.UserName != currentUser.UserName)
				currentUser.UserName = person.User.UserName;

			if (person.User.Email != currentUser.Email)
				currentUser.Email = person.User.Email;

			UploudImage(person, formCollection, currentUser);


			person.UserId = currentUser.Id;
			person.User = currentUser;

			personManager.EditPerson(person);
			SessionUser = currentUser;
			ViewBag.ImageName = person.ImageName;

            userManager.UpdateUser(SessionUser);
            

			return View(person);

		}

		private void UploudImage(PersonDTO person, FormCollection formCollection, UserDTO currentUser)
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
