using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helpers
{
    class HashingHelper
    {
        private static readonly int numberOfIterations = 50000;

        public static byte[] GenerateSalt(int saltLength)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            toBeHashed = HashingHelper.HashPasswordWithKeyDerivation(toBeHashed, salt, numberOfIterations);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed, salt));
            }
        }

        public static string GetHashedPasswordString(string inputPassword, string salt64string)
        {
            byte[] salt = Convert.FromBase64String(salt64string);
         
            // Hash Password
            byte[] userPasswordByteArr = Encoding.UTF8.GetBytes(inputPassword);
            byte[] hashedPassword = HashingHelper.HashPasswordWithSalt(userPasswordByteArr, salt);

            // Add Salt and Hashed Password to User
            return Convert.ToBase64String(hashedPassword);
        }

        private static byte[] HashPasswordWithKeyDerivation(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds))
            {
                return rfc2898.GetBytes(32);
            }
        }

    }
}
