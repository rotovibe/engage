using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResultsProcessor
{
    public class InvalidMessageException : ApplicationException
    {
        public InvalidMessageException(string message) : base(message)
        {
        }
    }
}
