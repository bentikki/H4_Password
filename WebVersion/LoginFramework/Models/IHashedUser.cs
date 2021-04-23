using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoginFramework.Models
{
    public interface IHashedUser
    {
        string Username { get; set; }
       
        [Required]
        string Password { get; set; }
        
        string Salt { get; set; }
        byte[] SaltByteArray { get; set; }
    }
}
