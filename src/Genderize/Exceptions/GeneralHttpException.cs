using System;

namespace Genderize.Exceptions
{
    public class GeneralHttpException : Exception
    {
        public GeneralHttpException()
        {
        }

        public GeneralHttpException(string message) : base(message)
        {
        }

        public GeneralHttpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
