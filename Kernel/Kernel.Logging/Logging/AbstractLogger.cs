namespace Kernel.Logging
{
	using System;
	using System.Diagnostics;
	using Kernel.Logging;

	/// <summary>
	/// Provides methods to log exception to the custom repository and event log
	/// </summary>
	/// <seealso cref="S3ID.Library.Logging.ILogProvider" />
    public abstract class AbstractLogger : ILogProvider
    {
        private bool _isDisposed;

        /// <summary>
        /// Implementation of LogWriter.
        /// </summary>
		public ILogWriter LogWriter { protected get; set; }
        
        /// <summary>
        /// Instance method to write an exception in the event log. Construct string with messages and stack trace from the exception
        /// and all inner exceptions(message and stack trace included).
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void WriteExceptionToEventLog(Exception exception)
        {
            Exception result;

            var appId = this.GetApplicationIdentity();

            AbstractLogger.TryWriteExceptionToEventLog(appId, exception, out result);
        }

        /// <summary>
        /// Instance method to write an string in the event log.
        /// </summary>
        /// <param name="eventInfo">String to write in the event log.</param>
        protected virtual void WriteToEventLog(string eventInfo, EventLogEntryType eventLogEntryType)
        {
            Exception result;

            var appId = this.GetApplicationIdentity();

            AbstractLogger.TryWriteToEventLog(appId, eventInfo, eventLogEntryType, out result);
        }

        /// <summary>
        /// Returns the Application Name. The dafault implementation returns friendly name of the application.
        /// </summary>
        /// <returns>The name of the application</returns>
        protected virtual string GetApplicationIdentity()
        {
            return AppDomain.CurrentDomain.FriendlyName.Replace(".exe", string.Empty).Trim();
        }

        /// <summary>
        /// Static method that tries to write an exception in the event log and return true or false in case of success or failure respectively. Result object contains the details of the exception
        /// trown during the process, null if it succeeds
        /// </summary>
        /// <param name="appId">The event log source. It has to be configured during the instalation. Otherwise the message will be logged against the default source
        /// provided by LogEventSourceProvider implementation</param>
        /// <param name="exception">An exeption to be logged. Construct string with messages and stack trace from the exception
        /// and all inner exceptions(message and stack trace included).</param>
        /// <param name="result">An object of type Exception with all exception thrown during the logging process.</param>
        /// <returns>True if the process succeeds, false otherwise. result object see result for details</returns>
        public static bool TryWriteExceptionToEventLog(string appId ,Exception exception, out Exception result)
        {
            string builtMessage = LoggerManager.BuildExceptionStringRecursively(exception);

            return AbstractLogger.TryWriteToEventLog(appId, builtMessage, EventLogEntryType.Error, out result);
        }

        /// <summary>
        /// Static method that tries to write an message in the event log and return true or false in case of success or failure respectively. Result object contains the details of the exception
        /// trown during the process, null if it succeeds
        /// </summary>
        /// provided by LogEventSourceProvider implementation</param>
        /// <param name="exception">An exeption to be logged. Construct string with messages and stack trace from the exception
        /// and all inner exceptions(message and stack trace included).</param>
        /// <param name="result">An object of type Exception with all exception thrown during the logging process.</param>
        /// <returns>True if the process succeeds, false otherwise. result object see result for details</returns>
        public static bool TryWriteToEventLog(string appId, string entry, EventLogEntryType eventLogEntryType, out Exception result)
        {
            result = null;

            try
            {
                var source = LogEventSourceProvider.GetSourceName(appId);

                var eventLog = new EventLog(LogEventSourceProvider.EventLogName);

                eventLog.Source = source;

                eventLog.WriteEntry(entry, eventLogEntryType);

                return true;
            }
            catch (Exception ex)
            {
                result = ex;

                return false;
            }
        }

        /// <summary>
        /// Basing Implementation of logging the exeption in the repository provided. The method try to commit the exception and write in the event log in
        /// case of failure. Returns true or false if success ot failure respectively. The Result object contains the details of the exception thrown during
        /// the process, null if success
        /// </summary>
        /// <param name="exceptionToLog">An exception to l;og</param>
        /// <param name="result">An object of type Exception providing details of the exception thrown during the process. Null if the process succeeds</param>
        /// <returns>True if success, false otherwise</returns>
        public virtual bool TryLogException(Exception exceptionToLog, out Exception result)
        {
            if (TryCommitException(exceptionToLog, out result))
                return true;

            var exceptionDetails = LoggerManager.BuildExceptionStringRecursively(result);

            var originalExeptionDetails = LoggerManager.BuildExceptionStringRecursively(exceptionToLog);

            this.WriteToEventLog(string.Format("An attempt to commit an exception: \r\n{0} to the repository has failed. The exception details are: {1}\r\n", originalExeptionDetails, exceptionDetails), EventLogEntryType.Error);

            return false;
        }

        /// <summary>
        /// Commit the object provided to the repository
        /// </summary>
        /// <param name="o">An object to log</param>
        /// <param name="exception">An object of type Exception providing details of the exception thrown during the process. Null if the process succeeds</param>
        /// <returns>True if success, false otherwise</returns>
        public virtual bool TryCommitException(object o, out Exception exception)
        {
            exception = null;

            try
            {
                if (this.LogWriter == null)
                    throw new NullReferenceException("No instance of ILogWriter is found. Make sure it is registered in DI contaiener and setter injection is allowed");

                this.LogWriter.WriteExeption(o);

                return true;
            }
            catch (Exception ex)
            {
                exception = ex;

                return false;
            }
        }

        /// <summary>
        /// Not implemented. Throws NotImplementedException.
        /// </summary>
        /// <param name="m">A message to log</param>
        public virtual void LogMessage(string m)
        {
            throw new NotImplementedException();
        }

        public virtual string AddParameter(string name, object value)
        {
            return null;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._isDisposed)
            {
                if (this.LogWriter == null)
                    return;

                var disposable = this.LogWriter as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }

            _isDisposed = true;
        }
    }
}