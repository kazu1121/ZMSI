using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMU
{
    /// <summary>
    /// A generic Neural Network exception.
    /// </summary>
    public class NetworkException : Exception
    {
        public NetworkException(string message)
            : base(message)
        {
        }

    }
}
