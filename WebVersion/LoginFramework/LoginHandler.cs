using System;
using LoginFramework.Models;
using LoginFramework.Helpers;
using LoginFramework.Hashing;

namespace LoginFramework
{
    public enum HashingMethod
    {
        SHA256
    }

    public class LoginHandler
    {

        private readonly IHashingMethod _hashingMethod;

        /// <summary>
        /// Login Handler
        /// </summary>
        /// <param name="hashingMethod">Which Hashing method to use.</param>
        public LoginHandler(HashingMethod hashingMethod = HashingMethod.SHA256)
        {
            IHashingMethod method = null;

            switch (hashingMethod)
            {
                case HashingMethod.SHA256:
                        method = new HashingSHA256();
                    break;
                default: throw new ArgumentException("Not a viable hashing method.", "hashingMethod");
            }

            this._hashingMethod = method;
        }

        /// <summary>
        /// Returns IHashedUser object, with hashed passowrd and salt.
        /// Takes Salt lenght as int - Default 32.
        /// </summary>
        /// <param name="username">Username string</param>
        /// <param name="password">Password string</param>
        /// <param name="saltSize">Salt lenght as int - default 32</param>
        /// <returns></returns>
        public IHashedUser CreateHashedUserInfo(string username, string password, int saltSize = 32)
        {
            // Check if user input is valid.
            if (username == string.Empty || username == "")
                throw new ArgumentNullException("Username");

            if (password == string.Empty || password == "")
                throw new ArgumentNullException("Password");

            // Generate secure salt.
            ISaltGenerator saltGenerator = new SaltGeneratorRNGCryptoService();
            byte[] salt = saltGenerator.GenerateSalt(saltSize);

            // Create return object.
            IHashedUser hashedUser = new HashedUser();
            hashedUser.Username = username; // Set username from user input.
            hashedUser.SaltByteArray = salt; // Set SaltByteArray with value generated above.

            // Instanziate hashing class - Select hashing method.
            hashedUser = this._hashingMethod.GetHashedUser(hashedUser); // Returns hashed user - inlcuding password.

            return hashedUser;
        }


    }
}
