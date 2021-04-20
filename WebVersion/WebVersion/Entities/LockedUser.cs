using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVersion.Entities
{
    public class LockedUser
    {
        public string UserName { get; set; }

        [Write(false)]
        [Computed]
        public string LockOutTime { get; set; }
    }
}