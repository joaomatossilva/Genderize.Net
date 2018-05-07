using System;

namespace Genderize.Exceptions
{
	public class InternalServerErrorException : GeneralHttpException
	{
		public InternalServerErrorException() : this("There is a problem with the server. They again later.")
		{
		}

		public InternalServerErrorException(string message) : base(message)
		{
		}

		public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
