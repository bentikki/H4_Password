using System;
using LoginFramework.Models;
using LoginFramework.Helpers;
using LoginFramework.Hashing;
using LoginFramework.HashingMethods;

namespace LoginFramework
{

    /// <summary>
    /// 
    /// </summary>
    public partial class LoginHandler
    {
        /// <summary>
        /// Checks if the Hashing method is supported.
        /// Throw exception if an unsupported method is chosen.
        /// Returns IHashingMethod if success.
        /// </summary>
        /// <param name="hashingMethod">HashingMethodType to check - returns the corresponding IHashingMethod.</param>
        /// <returns></returns>
        private IHashingMethod ViableHashingMethodCheck(HashingMethodType hashingMethod)
        {
            IHashingMethod method = null;

            switch (hashingMethod)
            {
                case HashingMethodType.SHA256:
                    method = new HashingSHA256();
                    break;
                default: throw new ArgumentException("Not a viable hashing method.", "hashingMethod");
            }

            return method;
        }

    }
}
