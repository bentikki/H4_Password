﻿using System;
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
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (UserService.Instance.IsUserLoggedIn())
            {
                return RedirectToAction("HomePage");
            }

            if (LoginAttemptHandler.MaxAttempts())
            {
                ViewBag.Error = "You are locked out due to too many login attempts.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string username, string password)
        {

            IUser user = new User();
            user.Username = username;
            user.UserPassword = password;

            try
            {
                user = UserService.Instance.Login(user);
                
                LoginAttemptHandler.ResetLoginAttempt();

                return RedirectToAction("HomePage");
            }
            catch (Exception e)
            {
                string errorText = string.Empty;

                LoginAttemptHandler.AddLoginAttempt();

                if (LoginAttemptHandler.MaxAttempts())
                {
                    errorText = "You have run out of login attempts.";
                }
                else
                {
                    errorText = "Could not login with provided credentials. Please try again.";
                }

                ViewBag.Error = errorText;
                return View();
            }
        }

        
        public ActionResult HomePage()
        {
            try
            {
                IUser loggedinUser = UserService.Instance.GetUser(User.Identity.Name);
                return View("HomePage", loggedinUser);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            UserService.Instance.LogOut();
            return RedirectToAction("Index");
        }

    }
}