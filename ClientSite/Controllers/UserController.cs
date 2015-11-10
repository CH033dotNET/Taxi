using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/
		public ActionResult Index()
		{

			var currentUser = SessionUser;

			if (currentUser == null)
				RedirectToAction("Registration", "Account");

			//var currentPerson = personManager.GetPersonByUserId(currentUser.Id);
			var currentPerson = ApiRequestHelper.Get<PersonDTO, int>("User", "GetPersonByUserId", currentUser.Id).Data as PersonDTO;

			if (currentPerson == null)
				RedirectToAction("Registration", "Account");

			return View(currentPerson);
		}

		[HttpPost]
		public ActionResult Index(PersonDTO person, FormCollection formCollection)
		{
			var currentUser = SessionUser;


			if (person.User.UserName != currentUser.UserName)
				currentUser.UserName = person.User.UserName;

			if (person.User.Email != currentUser.Email)
				currentUser.Email = person.User.Email;

			UploudImage(person, formCollection, currentUser);


			person.UserId = currentUser.Id;
			person.User = currentUser;

			//personManager.EditPerson(person);
			var myPerson = ApiRequestHelper.postData<PersonDTO>("User", "EditPerson", person).Data as PersonDTO;
			SessionUser = currentUser;
			ViewBag.ImageName = person.ImageName;




			return View(person);

		}

		private void UploudImage(PersonDTO person, FormCollection formCollection, UserDTO currentUser)
		{

			//person.ImageName = personManager.GetPersonByUserId(currentUser.Id).ImageName;
			var currentPerson = ApiRequestHelper.Get<PersonDTO, int>("User", "GetPersonByUserId", currentUser.Id).Data as PersonDTO;
			person.ImageName = currentPerson.ImageName;

			foreach (string item in Request.Files)
			{
				HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
				if (file.ContentLength == 0)
					continue;
				if (file.ContentLength > 0)
				{
					string mapImage = Server.MapPath(@"~\Images\") + person.ImageName;
					//ImageUpload imageUpload = new ImageUpload { Width = 200 };
					//delete last image
					if (file.FileName != person.ImageName)
					{
						var myResponse = ApiRequestHelper.postData<PersonDTO, string>("User", "ImageDelete", person, mapImage).Data as PersonDTO;
					}
						
						//imageUpload.DeleteFile(person.ImageName, profileAvatar, mapImage);

					// rename, resize, and upload
					//return object that contains {bool Success,string ErrorMessage,string ImageName}
					//ImageResult imageResult = imageUpload.RenameUploadFile(file);
					var imResult = ApiRequestHelper.postData<StringBuilder, HttpPostedFileBase>("User", "RenameUploadFile", file);
					if (imResult.StatusCode == HttpStatusCode.OK)
					{
						//TODO: write the filename to the db
						person.ImageName = imResult.Data.ToString();
					}
					else
					{
						// use imageResult.ErrorMessage to show the error
						ViewBag.Error = "Something wrong";
					}
				}
			}
		}
	}
}