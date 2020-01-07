using System;
using System.Collections.Generic;
using System.Text;

namespace BetEasy.Core.Exceptions
{
    public class InvalidFeedException : Exception
    {
        public InvalidFeedException()
        {
        }

        public InvalidFeedException(string message)
            : base(message)
        {
        }

        public InvalidFeedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
