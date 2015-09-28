namespace Testbed.Dapper
{
	using System;

	public class DuplicateEnumerationIdException : Exception
	{
		public DuplicateEnumerationIdException()
		{
		}

		public DuplicateEnumerationIdException(string message)
			: base(message)
		{
		}

		public DuplicateEnumerationIdException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
