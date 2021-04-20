using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Entities
{
    interface IUser
    {
        string Username { get; set; }
        string UserPassword { get; set; }
        string Salt { get; set; }
    }
}
