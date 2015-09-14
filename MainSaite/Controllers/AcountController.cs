using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class AcountController : BaseController
    {
        MainContext context = new MainContext();
        //
        // GET: /Acount/

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            if(ModelState.IsValid)
            {
                var role = context.Roles.Where(x => x.Name == AvailableRoles.User.ToString()).First();
                user.Role = role;
                user.RoleId = role.Id;
                context.Users.Add(user);
                context.SaveChanges();
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
                var existingAcount = context.Users.Include("Role").Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();
               
                if (existingAcount != null)
                {

                    Session["User"] = existingAcount;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong password or login");        
                }

              return View();
        }
    }
}
