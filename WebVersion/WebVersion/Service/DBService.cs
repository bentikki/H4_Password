using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleVersion.Entities;
using ConsoleVersion.Helpers;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using WebVersion.Entities;

namespace ConsoleVersion.Service
{
    class DBService
    {

        public void CreateUser(IUser user)
        {
            // Generate Salt
            int saltSize = 32;
            byte[] salt = HashingHelper.GenerateSalt(saltSize);
            string saltBaseString = Convert.ToBase64String(salt);

            // Add Salt and Hashed Password to User
            user.UserPassword = HashingHelper.GetHashedPasswordString(user.UserPassword, saltBaseString);
            user.Salt = saltBaseString;

            using (var conn = new SqlConnection(ConnectionStringHelper.GetDBConnectionString()))
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

                byte[] salt = Convert.FromBase64String(DBuser.Salt);
                byte[] insertedPasswordByteArr = Encoding.UTF8.GetBytes(user.UserPassword);
                byte[] hashedPassword = HashingHelper.HashPasswordWithSalt(insertedPasswordByteArr, salt);

                byte[] newHash = hashedPassword;

                string newHashString = Convert.ToBase64String(newHash);

                if(newHashString == DBuser.UserPassword)
                {
                    user.UserPassword = null;
                    user.Salt = null;

                    return user;
                }
                else{
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
                        new { 
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
