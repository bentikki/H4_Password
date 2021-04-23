using LoginFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginFramework.Hashing
{
    public interface IHashingMethod
    {
        byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt, int numberOfIterations = 50000);
        IHashedUser GetHashedUser(IHashedUser userToHash);
        string GetHashedPasswordString(string inputPassword, string salt64string);
    }
}
