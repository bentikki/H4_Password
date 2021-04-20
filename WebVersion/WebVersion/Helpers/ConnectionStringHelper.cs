using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helpers
{
    class ConnectionStringHelper
    {
        public static string GetDBConnectionString()
        {
            string connString = ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString;

            return connString;
        }
    }
}
