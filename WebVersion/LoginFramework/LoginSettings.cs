using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoginFramework
{
    public enum HashingMethodType
    {
        SHA256
    }

    /// <summary>
    /// Settings used to setup LoginHandler.
    /// </summary>
    public class LoginSettings
    {
        [Required]
        public HashingMethodType HashingMethod { get; set; }
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        public string TableName { get; set; }
        [Required]
        public string UsernameColumn { get; set; }
        [Required]
        public string PasswordColumn { get; set; }
        [Required]
        public string SaltColumn { get; set; }
        [Required]
        public int SaltSize { get; set; }

        public LoginSettings(string connectionString, string tableName, string usernameColumn, string passwordColumn, string saltColumn, HashingMethodType hashingMethod, int saltSize = 32)
        {
            HashingMethod = hashingMethod;
            ConnectionString = connectionString;
            TableName = tableName;
            UsernameColumn = usernameColumn;
            PasswordColumn = passwordColumn;
            SaltColumn = saltColumn;
            SaltSize = saltSize;
        }
    }
}
