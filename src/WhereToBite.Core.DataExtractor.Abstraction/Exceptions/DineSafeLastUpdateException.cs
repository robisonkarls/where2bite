using System;

namespace WhereToBite.Core.DataExtractor.Abstraction.Exceptions
{
    public class DineSafeLastUpdateException : Exception 
    {
        public DineSafeLastUpdateException()
        {
            
        }

        public DineSafeLastUpdateException(string message) : base(message)
        {
            
        }

        public DineSafeLastUpdateException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}