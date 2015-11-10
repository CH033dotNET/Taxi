using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/
		public ActionResult Registration()
		{
			return View();

		}

		[HttpPost]
		public ActionResult Registration(RegistrationModel user)
		{
			if (ModelState.IsValid)
			{
				//if (!userManager.IfUserNameExists(user.UserName) && !userManager.IfEmailExists(user.Email))
				bool ifUserNameExists = ApiRequestHelper.Get<bool, string>("Account", "IfUserNameExists", user.UserName).Data;
				bool ifEmailExists = ApiRequestHelper.Get<bool, string>("Account", "IfEmailExists", user.UserName).Data;
				if (!ifUserNameExists && !ifEmailExists)
				{
					bool isUserNameCorrect = ApiRequestHelper.Get<bool, string>("Account", "IsUserNameCorrect", user.UserName).Data;
					if (isUserNameCorrect)
					{
						ApiRequestHelper.PutObject<UserDTO>("Account", "InsertUser", user);
						//userManager.InsertUser(user);
						SessionPerson = null;
						SessionUser = user;
						CheckPerson();
						Authentification(new LoginModel() { UserName = user.UserName, Password = user.Password });
						return RedirectToAction("Index", "User");
					}
					//else ModelState.AddModelError("", "Login syntax error");
				}
				//else ModelState.AddModelError("", "User Name or Email is already exist");
			}

			return View(user);
		}

		public ActionResult Authentification()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Authentification(LoginModel user)
		{
			if (ModelState.IsValid)
			{
				//var currentUser = userManager.GetByUserName(user.UserName, user.Password);
				SessionUser =
					ApiRequestHelper.postData<UserDTO, LoginModel>("Account", "GetUser", user).Data as UserDTO;
				if (SessionUser != null)
				{
					CheckPerson();
					SessionPerson = ApiRequestHelper.Get<PersonDTO, int>("Account", "getPerson", SessionUser.Id).Data as PersonDTO;
					//SessionPerson = personManager.GetPersonByUserId(currentUser.Id);
					return RedirectToAction("Index", "Home");
				}
				else ModelState.AddModelError("", "Wrong password or login");
			}

			return View();
		}

		public ActionResult LogOut()
		{
			Session["User"] = null;
			return RedirectToAction("Index", "Home");
		}
		/// <summary>
		/// Not support browsers less than IE9
		/// </summary>
		/// <returns></returns>
		public ActionResult IEDisable()
		{
			return View();
		}

		private void CheckPerson()
		{
			//var currentUser = userManager.GetByUserName(user.UserName, user.Password);
			//var currentPerson = personManager.GetPersonByUserId(currentUser.Id);

			if (SessionPerson == null)
			{
				PersonDTO myPerson = new PersonDTO() { UserId = SessionUser.Id, ImageName = "item_0_profile.jpg" };
				//currentPerson = personManager.InsertPerson(new PersonDTO() { UserId = currentUser.Id, ImageName = "item_0_profile.jpg" });
				SessionPerson = ApiRequestHelper.postData<PersonDTO>("Account", "InsertPerson", myPerson).Data as PersonDTO;
				SessionPerson.User = SessionUser;
			}
			if (!System.IO.File.Exists(Server.MapPath(@"~\Images\") + SessionPerson.ImageName))
			{
				ApiRequestHelper.GetById<PersonDTO>("Account", "DefaultImage", SessionUser.Id);
				//personManager.DefaultImage(user.Id);

			}
		}
	}
}