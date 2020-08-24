using System;

namespace WhereToBite.Domain.Exceptions
{
    public class WhereToBiteDomainException : Exception
    {
        public WhereToBiteDomainException()
        {
            
        }

        public WhereToBiteDomainException(string message) : base(message)
        {
            
        }

        public WhereToBiteDomainException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}