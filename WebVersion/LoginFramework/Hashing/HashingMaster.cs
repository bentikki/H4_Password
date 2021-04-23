using LoginFramework.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LoginFramework.Hashing
{
    public abstract class HashingMaster
    {
        public abstract byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt, int numberOfIterations = 50000);

        /// <summary>
        /// Returns IHashedUser object with password and salt hashed.
        /// The IHashedUser object must include Username, Password, and SaltByteArray.
        /// </summary>
        /// <param name="userToHash">User to hash</param>
        /// <returns></returns>
        public IHashedUser GetHashedUser(IHashedUser userToHash)
        {
            // Check if user input is valid.
            if (userToHash.Username == string.Empty || userToHash.Username == "")
                throw new ArgumentNullException("Username");

            if (userToHash.Password == string.Empty || userToHash.Password == "")
                throw new ArgumentNullException("Password");

            if (userToHash.SaltByteArray == null || userToHash.SaltByteArray.Length <= 0)
                throw new ArgumentNullException("SaltByteArray");

            // Convert password string to byte array
            byte[] passwordByteArray = Encoding.UTF8.GetBytes(userToHash.Password);

            // Create hashed password with salt.
            byte[] passwordWithSalt = this.HashPasswordWithSalt(passwordByteArray, userToHash.SaltByteArray);

            // Set salt string - using Base64String
            userToHash.Password = Convert.ToBase64String(passwordWithSalt);
            userToHash.Salt = Convert.ToBase64String(userToHash.SaltByteArray);

            return userToHash;
        }

        public string GetHashedPasswordString(string inputPassword, string salt64string)
        {
            byte[] salt = Convert.FromBase64String(salt64string);

            // Hash Password
            byte[] userPasswordByteArr = Encoding.UTF8.GetBytes(inputPassword);
            byte[] hashedPassword = this.HashPasswordWithSalt(userPasswordByteArr, salt);

            // Add Salt and Hashed Password to User
            return Convert.ToBase64String(hashedPassword);
        }

        protected byte[] HashPasswordWithKeyDerivation(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
            {
                return rfc2898.GetBytes(32);
            }
        }

        protected byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }
    }
}
