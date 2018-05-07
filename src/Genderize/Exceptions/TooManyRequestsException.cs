using System;
using System.Collections.Generic;
using System.Text;

namespace Genderize.Exceptions
{
	public class TooManyRequestsException : GeneralHttpException
	{
		public TooManyRequestsException() : this("You need to slow down a bit")
		{
		}

		public TooManyRequestsException(string message) : base(message)
		{
		}

		public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
