using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVersion.Helpers
{
    class ConnectionStringHelper
    {
        public static string GetDBConnectionString()
        {
            string connString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PasswordHashing;Integrated Security=SSPI;";

            return connString;
        }
    }
}
