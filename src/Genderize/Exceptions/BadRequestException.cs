using System;

namespace Genderize.Exceptions
{
    public class BadRequestException : GeneralHttpException
    {
        public BadRequestException() : this("There was a problem with your request")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
