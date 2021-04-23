using System;
using System.Collections.Generic;
using System.Text;

namespace LoginFramework.Helpers
{
    interface ISaltGenerator
    {
        byte[] GenerateSalt(int saltLength);
    }
}
