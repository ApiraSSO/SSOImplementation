namespace Kernel.Logging
{
	using System;

	/// <summary>
	/// Provides methods to log information and exceptions
	/// </summary>
	public interface ICustomLogger
	{
		/// <summary>
		/// Use for debugging level logging
		/// </summary>
		/// <param name="message">The message.</param>
		void Debug(string message);

		/// <summary>
		/// Use for debugging level logging
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="ex">The ex.</param>
		void Debug(string message, Exception ex);

		/// <summary>
		/// Use for information level logging
		/// </summary>
		/// <param name="message">The message.</param>
		void Info(string message);

		/// <summary>
		/// Use for information level logging
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="ex">The ex.</param>
		void Info(string message, Exception ex);

		/// <summary>
		/// Use for warn level logging
		/// </summary>
		/// <param name="message">The message.</param>
		void Warn(string message);

		/// <summary>
		/// Use for warn level logging
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="ex">The ex.</param>
		void Warn(string message, Exception ex);

		/// <summary>
		/// Use for error level logging
		/// </summary>
		/// <param name="message">The message.</param>
		void Error(string message);

		/// <summary>
		/// Use for error level logging
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="ex">The ex.</param>
		void Error(string message, Exception ex);

		/// <summary>
		/// Use for fatal error logging
		/// </summary>
		/// <param name="message">The message.</param>
		void Fatal(string message);

		/// <summary>
		/// Use for fatal error logging
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="ex">The ex.</param>
		void Fatal(string message, Exception ex);
	}
}
