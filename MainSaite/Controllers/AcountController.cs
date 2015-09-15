using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.Manager;

namespace MainSaite.Controllers
{
    public class AcountController : BaseController
    {
		UnitOfWork unit = new UnitOfWork();
		UserManager userManager = null;

		public AcountController()
		{
			userManager = new UserManager(unit);
		}

        //
        // GET: /Acount/

        public ActionResult Registration()
        {
            return View();
        }

		[HttpPost]
		public ActionResult Registration(User user)
		{
			if (ModelState.IsValid)
			{
				
				bool isInserted = userManager.InsertUser(user);
				if (!isInserted) { ModelState.AddModelError("", "User is already exist"); }
			}
			return View();
		}

        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(User user)
		{		
			var existingAcount = userManager.UserAuth(user);               
                if (existingAcount != null)
                {
                    Session["User"] = existingAcount;
                    return RedirectToAction("Index", "Home");
                }       
				else  ModelState.AddModelError("", "Wrong password or login");        
                
              return View();
        }
    }
}
