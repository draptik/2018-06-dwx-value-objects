using System;

namespace Examples.Exceptions
{
    public class InvalidGeld2ValueException : Exception
    {
        public InvalidGeld2ValueException(string message) : base(message)
        {
        }
    }
}