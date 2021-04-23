using ConsoleVersion.Helpers;
using LoginFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebVersion.Helpers
{
    public static class HandlerFactory
    {
        public static LoginHandler GetLoginHandler()
        {
            LoginSettings settings = new LoginSettings(HandlerFactory.GetDBConnectionString(), "Users", "UserName", "UserPassword", "Salt", HashingMethodType.SHA256);
            LoginHandler loginHandler = new LoginHandler(settings);

            return loginHandler;
        }

        public static string GetDBConnectionString()
        {
            string connString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;

            return connString;
        }
    }
}