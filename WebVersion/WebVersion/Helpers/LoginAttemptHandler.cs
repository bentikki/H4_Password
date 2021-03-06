using ConsoleVersion.Entities;
using ConsoleVersion.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebVersion.Service;

namespace ConsoleVersion.Helpers
{
    static class LoginAttemptHandler
    {
        private static readonly int loginAttemptsMax = 3;
        private static int currentAttempts = 0;

        public static void AddLoginAttempt()
        {
            LoginAttemptHandler.currentAttempts++;
        }

        public static void ResetLoginAttempt()
        {
            LoginAttemptHandler.currentAttempts = 0;
        }

        public static bool MaxAttempts()
        {
            if(LoginAttemptHandler.currentAttempts >= LoginAttemptHandler.loginAttemptsMax)
            {
                return true;
            }

            return false;
        }

        public static void LockOutUser(string username)
        {
            UserService.Instance.LockOutUser(username);
            LoginAttemptHandler.ResetLoginAttempt();
        }

    }
}
