using System;

namespace Kernel.Logging
{
    /// <summary>
    /// Log providers members
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ILogProvider : IDisposable
    {
		/// <summary>
		/// Sets the log writer.
		/// </summary>
		/// <value>
		/// The log writer.
		/// </value>
        ILogWriter LogWriter { set; }

		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="m">The m.</param>
        void LogMessage(string m);

		/// <summary>
		/// Tries the log exception.
		/// </summary>
		/// <param name="ex">The ex.</param>
		/// <param name="result">The result.</param>
		/// <returns></returns>
        bool TryLogException(Exception ex, out Exception result);

		/// <summary>
		/// Tries the commit exception.
		/// </summary>
		/// <param name="o">The o.</param>
		/// <param name="exception">The exception.</param>
		/// <returns></returns>
        bool TryCommitException(object o, out Exception exception);

		/// <summary>
		/// Adds the parameter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
        string AddParameter(string name, object value);
    }
}