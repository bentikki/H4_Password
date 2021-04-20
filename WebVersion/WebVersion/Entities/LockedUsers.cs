using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVersion.Entities
{
    public class LockedUsers
    {
        public string UserName { get; set; }
        public string LockOutTime { get; set; }
    }
}