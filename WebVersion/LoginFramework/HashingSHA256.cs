using LoginFramework.Hashing;
using LoginFramework.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LoginFramework
{
    public class HashingSHA256 : HashingMaster, IHashingMethod
    {
        public override byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt, int numberOfIterations = 50000)
        {
            toBeHashed = this.HashPasswordWithKeyDerivation(toBeHashed, salt, numberOfIterations);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed, salt));
            }
        }
    }
}
