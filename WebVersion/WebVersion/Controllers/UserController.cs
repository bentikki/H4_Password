using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using ConsoleVersion.Entities;
using ConsoleVersion.Helpers;
using ConsoleVersion.Service;
using WebVersion.Service;

namespace WebVersion.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [AllowAnonymous]
        public ActionResult CreateUser()
        {
            if (UserService.Instance.IsUserLoggedIn())
            {
                return RedirectToAction("HomePage", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateUser(string username, string password)
        {
            if (UserService.Instance.IsUserLoggedIn())
            {
                return RedirectToAction("HomePage", "Home");
            }
            IUser user = new User();
            user.Username = username;
            user.UserPassword = password;

            try
            {
                UserService.Instance.CreateUser(user);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                if(e.Message == "Username_exists")
                {
                    ViewBag.Error = "The selected username already exists.";
                }
                else
                {
                    ViewBag.Error = "An error has occured. User could not be created.";
                }
                return View();
            }
            
        }

    }
}