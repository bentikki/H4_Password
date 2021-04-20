using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Entities
{
    class User : IUser
    {
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string Salt { get; set; }


    }
}
