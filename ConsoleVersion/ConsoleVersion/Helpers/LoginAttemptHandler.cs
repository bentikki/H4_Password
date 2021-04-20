using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helpers
{
    static class LoginAttemptHandler
    {
        private static readonly int loginAttemptsMax = 5;
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
    }
}
