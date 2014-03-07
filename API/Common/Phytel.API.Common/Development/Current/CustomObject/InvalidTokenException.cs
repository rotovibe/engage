using System;
namespace Phytel.API.Common.CustomObject
{
    public class InvalidTokenException : Exception
    {
        // The default constructor needs to be defined
        // explicitly now since it would be gone otherwise.

        public InvalidTokenException()
        {
        }

        public InvalidTokenException(string message)
            : base(message)
        {
        }
    }
}
