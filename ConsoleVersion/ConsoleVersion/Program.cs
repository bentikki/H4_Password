using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleVersion.Entities;
using ConsoleVersion.Service;
using ConsoleVersion.Helpers;

namespace ConsoleVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            DBService dBService = new DBService();
            bool mainMenuContinue = true;
            bool userLockedOut = false;


            do
            {
                IUser currentUser;
                string menuSelection;

                Console.WriteLine("----------Password Hashing----------");
                Console.WriteLine("Start Menu:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Create User");

                menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        bool loginUserSuccess = false;
                        int loginAttempts = 0;
                        do
                        {
                            try
                            {

                                Console.Write("Username: ");
                                string userInputUsername = Console.ReadLine();

                                Console.Write("Password: ");
                                string userInputPassword = Console.ReadLine();

                                currentUser = new User();
                                currentUser.Username = userInputUsername;
                                currentUser.UserPassword = userInputPassword;

                                IUser loggedInUser = dBService.LoginUser(currentUser);

                                loginUserSuccess = true;

                                if (loginUserSuccess)
                                {
                                    Console.WriteLine("Login success!");
                                    Console.WriteLine("Logged in username: " + loggedInUser.Username);
                                    LoginAttemptHandler.ResetLoginAttempt();
                                }
                            }
                            catch (Exception e)
                            {
                                loginUserSuccess = false;
                                Console.WriteLine("An error occured while attempting login. Please try again...");

                                LoginAttemptHandler.AddLoginAttempt();

                                if (LoginAttemptHandler.MaxAttempts())
                                {
                                    userLockedOut = true;
                                }
                            }
                        } while (!loginUserSuccess && !userLockedOut);

                        break;
                    case "2":
                        bool createUserSuccess = false;
                        do
                        {
                            try
                            {
                                Console.Write("Username: ");
                                string userInputUsername = Console.ReadLine();

                                Console.Write("Password: ");
                                string userInputPassword = Console.ReadLine();

                                currentUser = new User();
                                currentUser.Username = userInputUsername;
                                currentUser.UserPassword = userInputPassword;

                                dBService.CreateUser(currentUser);

                                createUserSuccess = true;
                            }
                            catch (Exception e)
                            {
                                createUserSuccess = false;
                                Console.WriteLine("An error occured while creating user. Please try again...");
                            }
                        } while (!createUserSuccess && !userLockedOut);


                        break;
                    default:
                        break;
                }
            } while (mainMenuContinue && !userLockedOut);


            if (userLockedOut)
            {
                Console.WriteLine($"You have been locked out after too many attempts...");
            }

            Console.WriteLine("Program has ended. Press any key to exit...");
            Console.ReadKey();

        }
    }
}
