using System;
using System.Collections.Generic;
using System.Text;

namespace LoginFramework.Models
{
    public interface IHashedUser
    {
        string Username { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        byte[] SaltByteArray { get; set; }
    }
}
