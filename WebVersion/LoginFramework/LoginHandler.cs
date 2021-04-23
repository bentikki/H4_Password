using LoginFramework.Hashing;
using LoginFramework.Helpers;
using LoginFramework.Models;
using System;
using System.Text;

namespace LoginFramework
{
    /// <summary>
    ///
    /// </summary>
    public partial class LoginHandler
    {
        private readonly IHashingMethod _hashingMethod;
        private readonly LoginSettings _settings;

        /// <summary>
        /// Used to setup login functionality.
        /// Takes LoginSettings as argument.
        /// </summary>
        /// <param name="settings">Settings used to setup login.</param>
        public LoginHandler(LoginSettings settings)
        {
            this._settings = settings;
            this._hashingMethod = this.ViableHashingMethodCheck(this._settings.HashingMethod);
        }

        /// <summary>
        /// Returns IHashedUser object, with hashed passowrd and salt.
        /// Takes Salt lenght as int - Default 32.
        /// </summary>
        /// <param name="username">Username string</param>
        /// <param name="password">Password string</param>
        /// <param name="saltSize">Salt lenght as int - default 32</param>
        /// <returns></returns>
        public IHashedUser CreateHashedUserInfo(string username, string password)
        {
            // Check if user input is valid.
            if (username == string.Empty || username == "")
                throw new ArgumentNullException("Username");

            if (password == string.Empty || password == "")
                throw new ArgumentNullException("Password");

            // Generate secure salt.
            ISaltGenerator saltGenerator = new SaltGeneratorRNGCryptoService();
            byte[] salt = saltGenerator.GenerateSalt(this._settings.SaltSize);

            // Create return object.
            IHashedUser hashedUser = new HashedUser();
            hashedUser.Username = username; // Set username from user input.
            hashedUser.Password = password;
            hashedUser.Salt = Convert.ToBase64String(salt); // Set SaltByteArray with value generated above.

            // Instanziate hashing class - Select hashing method.
            hashedUser = this._hashingMethod.GetHashedUser(hashedUser); // Returns hashed user - inlcuding password.

            return hashedUser;
        }

        /// <summary>
        /// Verify the integrity of the User Salt and password.
        /// Returns true if the password passes.
        /// Returns false if the hashed password and salt does not match.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public bool VerifyUserHash(IHashedUser userLogin, IHashedUser DBuser)
        {
            bool userHashIsVerified = false;

            // Make sure the Database Salt is set.
            if (DBuser.Salt == null || DBuser.Salt == string.Empty || DBuser.Salt == "")
                throw new ArgumentException("User 1 salt is not set", "user1");

            // Converts 64ByteString to ByteArray - IHashedUser.SaltByteArray
            DBuser.SaltByteArray = Convert.FromBase64String(DBuser.Salt);
            
            // Hash logged in user.
            IHashedUser hashedUserLogin = this._hashingMethod.GetHashedUser(userLogin);

            if (hashedUserLogin.Password == DBuser.Password)
            {
                userHashIsVerified = true;
            }

            return userHashIsVerified;
        }

        /// <summary>
        /// Checks if the password without salt, mathes DB password
        /// </summary>
        /// <param name="inputPassword"></param>
        /// <param name="passwordWithSalt"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public bool VerifyPasswordHash(string inputPassword, string passwordWithSalt, string salt)
        {
            bool hashedSucceded = false;

            // Hash new password with stored password salt.
            string newPasswordHash = this._hashingMethod.GetHashedPasswordString(inputPassword, salt);

            // If new hash matches old hash - return true.
            if(newPasswordHash == passwordWithSalt)
                hashedSucceded = true;
            
            return hashedSucceded;

        }

    }
}