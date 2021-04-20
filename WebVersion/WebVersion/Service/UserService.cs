using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ConsoleVersion.Entities;
using ConsoleVersion.Service;

namespace WebVersion.Service
{
    public class UserService
    {
        private DBService dbService;
        private static readonly UserService instance = new UserService();
        private UserService() 
        {
            this.dbService = new DBService();
        }
        public static UserService Instance
        {
            get
            {
                return instance;
            }
        }
        
        public bool IsUserLoggedIn()
        {
            bool userIsLoggedIn = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            return userIsLoggedIn;
        }

        public IUser Login(IUser user)
        {
            IUser selectedUser = this.dbService.LoginUser(user);
            if (selectedUser == null)
                throw new Exception("No_User_Found");

            FormsAuthentication.SetAuthCookie(selectedUser.Username, false);
            return selectedUser;
        }

        public IUser GetUser(string userName)
        {
            IUser user = this.dbService.GetUserByUsername(userName);

            return user;
        }

        public void CreateUser(IUser user)
        {
            IUser userNameExist = this.dbService.GetUserByUsername(user.Username);

            if (userNameExist != null)
                throw new Exception("Username_exists");

            this.dbService.CreateUser(user);

        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}