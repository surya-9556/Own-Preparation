using EmployeeSalaryPredc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmployeeSalaryPredc.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (PredictionEntities PE = new PredictionEntities())
            {
                bool isValied = PE.Users.Any(i => i.UserName == user.UserName && i.Password == user.Password);
                if (isValied)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName,false);
                    return RedirectToAction("Index", "Employee");
                }
                ModelState.AddModelError("", "Invalied username or password");
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}