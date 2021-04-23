
using System;

namespace LoginFramework.Models
{
    public class HashedUser : IHashedUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public byte[] SaltByteArray { get; set; }
    }
}
