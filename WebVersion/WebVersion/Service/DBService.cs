using ConsoleVersion.Entities;
using ConsoleVersion.Helpers;
using Dapper;
using Dapper.Contrib.Extensions;
using LoginFramework;
using LoginFramework.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebVersion.Entities;
using WebVersion.Helpers;

namespace ConsoleVersion.Service
{
    internal class DBService
    {
        public void CreateUser(IUser user)
        {
            IHashedUser hashedUser = HandlerFactory.GetLoginHandler().CreateHashedUserInfo(user.Username, user.UserPassword);

            user.UserPassword = hashedUser.Password;
            user.Salt = hashedUser.Salt;

            using (var conn = new SqlConnection(HandlerFactory.GetDBConnectionString()))
            {
                conn.Open();
                var identity = conn.Insert(user);
                conn.Close();
            }
        }

        public IUser LoginUser(IUser user)
        {
            try
            {
                IUser DBuser = this.GetUserByUsername(user.Username);

                if (DBuser == null)
                    throw new Exception("LoginError_User_Not_Found");

                bool userIsVerified = HandlerFactory.GetLoginHandler().VerifyPasswordHash(user.UserPassword, DBuser.UserPassword, DBuser.Salt);

                if (userIsVerified)
                {
                    user.UserPassword = null;
                    user.Salt = null;

                    return user;
                }
                else
                {
                    throw new Exception("LoginError_Password_Incorrect");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IUser GetUserByUsername(string fetchUsername)
        {
            IUser user;
            using (var conn = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                conn.Open();
                user = conn.QuerySingleOrDefault<User>(
                        "SELECT * FROM Users WHERE [UserName] = @Username",
                        new
                        {
                            Username = fetchUsername
                        }
                    );
                conn.Close();
            }
            return user;
        }

        public void LockOutUser(string username)
        {
            LockedUser lockUser = new LockedUser();
            lockUser.UserName = username;

            using (var conn = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                conn.Open();
                var identity = conn.Insert<LockedUser>(lockUser);
                conn.Close();
            }
        }

        public LockedUser IsUserLockedOut(string username)
        {
            LockedUser userIslocked;

            this.ClearLockedOut();

            using (var conn = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                string sql = "SELECT * FROM [PasswordHashing].[dbo].[LockedUsers] WHERE [UserName] = @Username;";

                conn.Open();
                userIslocked = conn.Query<LockedUser>(sql, new { Username = username }).FirstOrDefault();
                conn.Close();
            }

            return userIslocked;
        }

        private void ClearLockedOut()
        {
            using (var conn = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
            {
                string sql = "DELETE FROM [PasswordHashing].[dbo].[LockedUsers] WHERE [LockOutTime] < DATEADD(MINUTE, -1, GETDATE())";

                conn.Open();
                conn.Query(sql);
                conn.Close();
            }
        }
    }
}