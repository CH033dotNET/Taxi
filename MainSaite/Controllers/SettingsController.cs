using Common.Enum;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL.UsersRolesHellpers;


namespace MainSaite.Controllers
{
    public class SettingsController : Controller
    {
        MainContext db = new MainContext();
        //
        // GET: /Settings/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UsersMenu()
        {
            //role controll, does not working until we have autorizations
          ChekWhithRedirect(AvailableRoles.Administrator.ToString() + AvailableRoles.Operator.ToString());

            var users = new MainContext().Users.AsNoTracking().Include("Role").ToList();

            return View(users);
        }

        public ActionResult ChangeMenu(int id = 0)
        {
            //role controll, does not working until we have autorizations
            ChekWhithRedirect(AvailableRoles.Administrator.ToString() + AvailableRoles.Operator.ToString());

            var user = new MainContext().Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult ChangeMenu(User user)
        {

            //role controll, does not working until we have autorizations
        //    ChekWhithRedirect(AvailableRoles.Administrator.ToString() + AvailableRoles.Operator.ToString());

            if (ModelState.IsValid)
            {
                ///Think about this 3 strings
                Role role = db.Roles.Where(x => x.Name == user.Role.Name).First();
                user.Role = role;
                user.RoleId = role.Id;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UsersMenu");
            }

            return View(user);
        }

        public ActionResult ChekWhithRedirect(string validRoles)
        {
            
            Role currrent = new Role();

            if (Session["User"]!=null)
                currrent = new Role(((User)Session["User"]).Role);

            ///переадрисация на еррор страницу, которой еще нет
            if (!currrent.RoleControll(validRoles))
            {
                //throw new NotImplementedException();
                return View("Error");
            }
			return View("Error");
        }

    }
}
