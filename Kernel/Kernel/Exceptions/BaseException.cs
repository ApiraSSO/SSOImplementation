namespace Kernel.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Base class for custom exception
	/// </summary>
	/// <seealso cref="System.Exception" />
	public abstract class BaseException : Exception
	{
		public BaseException()
		{
		}

		public BaseException(string message)
			: base(message)
		{
		}

		public BaseException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected BaseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}