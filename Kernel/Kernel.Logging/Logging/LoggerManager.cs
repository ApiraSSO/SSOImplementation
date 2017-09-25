namespace Kernel.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Kernel.Configuration;
    using Kernel.Initialisation;

    public class LoggerManager : IDisposable
    {
        /// <summary>
        /// Retains the the statistics of exception of given type.
        /// </summary>
        //private static Dictionary<Type, ExceptionStatistics> _exceptionStatistics = new Dictionary<Type, ExceptionStatistics>();
        
        /// <summary>
        /// Implementation of Unity logger
        /// </summary>
        private ILogProvider _logger;
        
        /// <summary>
        /// Aplication name.
        /// </summary>
        private static string _applicationName;
        
        /// <summary>
        /// Logger property if Logger manager is build from DI container
        /// </summary>
        public ILogProvider Logger { set { _logger = value; } }

        /// <summary>
        /// Static consdtuctor to initialise services
        /// </summary>
        static LoggerManager()
        {
            _applicationName = GetApplicationName();
        }

        /// <summary>
        /// Enqueues the exception synchronously.
        /// </summary>
        /// <param name="exceptionToLog">The exception to log.</param>
        /// <param name="arguments">The arguments.</param>
        public void EnqueueExceptionSync(Exception exceptionToLog, params object[] arguments)
        {
            try
            {
                SendNotificationSync(exceptionToLog);

                if (!TryBuildLogger(exceptionToLog))
                    return;

                LogExceptionInternal(exceptionToLog, arguments);

            }
            catch (Exception ex)
            {
                HandleExceptionFromLoggerAndNotification(ex);
            }
        }

        /// <summary>
        /// Enqueues the exception asynchronously.
        /// </summary>
        /// <param name="exceptionToLog">The exception to log.</param>
        /// <param name="arguments">The arguments.</param>
        public async Task EnqueueExceptionAsync(Exception exceptionToLog, params object[] arguments)
        {
            try
            {
                var emailTask = SendNotificationAsync(exceptionToLog);

                if (!TryBuildLogger(exceptionToLog))
                {
                    await emailTask;

                    return;
                }

                var loggingTask = Task.Factory.StartNew(() =>
                {
                    LogExceptionInternal(exceptionToLog, arguments);
                });

                await Task.WhenAll(new Task[] { loggingTask, emailTask });
            }
            catch (Exception ex)
            {
                HandleExceptionFromLoggerAndNotification(ex);
            }
        }

        /// <summary>
        /// Write the exception to Unity event log
        /// </summary>
        /// <param name="exceptionToLog"></param>
        public static bool WriteExceptionToEventLog(Exception exceptionToLog)
        {
            if (_applicationName == null)
                _applicationName = GetApplicationName();

            Exception resultException;
            
            var result = AbstractLogger.TryWriteExceptionToEventLog(_applicationName ?? string.Empty, exceptionToLog, out resultException);

            return result;
        }

        /// <summary>
        /// Write information to Unity event log
        /// </summary>
        /// <param name="info"></param>
        public static bool WriteInformationToEventLog(string info)
        {
            if (_applicationName == null)
                _applicationName = GetApplicationName();

            Exception resultException;

            var result = AbstractLogger.TryWriteToEventLog(_applicationName ?? string.Empty, info, EventLogEntryType.Information, out resultException);

            return result;
        }

        /// <summary>
        /// Write a warning to Unity event log
        /// </summary>
        /// <param name="info"></param>
        public static bool WriteWarningToEventLog(string info)
        {
            if (_applicationName == null)
                _applicationName = GetApplicationName();

            Exception resultException;

            var result = AbstractLogger.TryWriteToEventLog(_applicationName ?? string.Empty, info, EventLogEntryType.Warning, out resultException);

            return result;
        }

        /// <summary>
        /// Register a custom appender to N Service Bus logging system
        /// </summary>
        public static void RegisterAppender()
        {
            throw new NotImplementedException();
            //SetLoggingLibrary.Log4Net(null, new NBusAppender());
        }

        /// <summary>
        /// /// Register a custom logger to N Service Bus logging system
        /// </summary>
        public static void RegisterLoggerFactory()
        {
            throw new NotImplementedException();
            //SetLoggingLibrary.Custom(new CustomNServiceBusLoggerFactory());
        }

        /// <summary>
        /// Builds human readable text from the exception. The text comprises of type of the exception, message, and stack trace. All inner exceptions are included. Aggregate exceptions are flattened.
        /// </summary>
        /// <param name="ex">Exception for which the text is to be built.</param>
        /// <returns>The text representing the exception details including all inner exceptions.</returns>
        public static string BuildExceptionStringRecursively(Exception ex)
        {
            if (ex == null)
                return null;

            var stringBuilder = new StringBuilder();

            Func<Exception, bool> BuildInnerExceptionDetail = e =>
            {
                var innerException = e;

                while (innerException != null)
                {

                    stringBuilder.AppendLine("Inner Exception:");

                    AddToExceptionDeails(stringBuilder, innerException);

                    innerException = innerException.InnerException;

                }

                return true;
            };

            AddToExceptionDeails(stringBuilder, ex);

            if (ex is AggregateException)
            {
                var ae = ((AggregateException)ex).Flatten();

                ae.Handle(BuildInnerExceptionDetail);

                return stringBuilder.ToString();
            }

            if (ex.InnerException != null)
                BuildInnerExceptionDetail(ex.InnerException);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Sends an email to all recipients defined in exception settings provoded the time since the last notification was sent has expired.
        /// </summary>
        /// <param name="exception"></param>
        protected async Task SendNotificationAsync(Exception exception)
        {
            var t = Task.Factory.StartNew(e =>
            {
                SendEmailForException(e);
            }, exception);
            
            await t;
        }

        /// <summary>
        /// Sends the notification synchronously.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected void SendNotificationSync(Exception exception)
        {
            try
            {
                SendEmailForException(exception);
            }
            catch (Exception ex)
            {
                HandleExceptionFromLoggerAndNotification(ex);
            }
        }

        /// <summary>
        /// Sends an email to defined recipients
        /// </summary>
        /// <param name="exception"></param>
        public static void SendEmailForException(object exception)
        {
			//var notificationProvider = new ExceptionNotificationSettingsProvider();

			//if (!notificationProvider.IsInitialised)
			//	notificationProvider.WaitToInitialise();

			//if (((SettingsProviderBase)notificationProvider).HasError)
			//{
			//	var providerError = ((SettingsProviderBase)notificationProvider).Error;

			//	var exceptionDetails = BuildExceptionStringRecursively(providerError);

			//	WriteWarningToEventLog(string.Format("Notification provider has failed to initialise. Error details are:\r\n{0}.", exceptionDetails));

			//	return;
			//}

			//var ex = exception as Exception;

			//if (ex == null)
			//{
			//	throw new ArgumentException("The object has to derive from exception", "Exception");
			//}

			//var settings = FindSetting(ex);

			//if(settings == null)
			//{
			//	return;
			//}

			//if (_exceptionStatistics.ContainsKey(settings.NotificationEntryType))
			//{
			//	var lastSentEmailTime = _exceptionStatistics[settings.NotificationEntryType].LastNotificationSentOn;

			//	if (settings.TimeSpan > TimeSpan.MinValue && DateTime.Now.Subtract(lastSentEmailTime) < settings.TimeSpan)
			//		return;
			//}

			//var mailMessage = ExceptionNotificationSettingsProvider.EmailMessageClone;

			////should override the recipients or add them as CC
			//if (settings.To != null && settings.To.Count() > 0)
			//	mailMessage.To = settings.To;

			//var exceptionDetail = BuildExceptionStringRecursively(ex);

			//var stackTrace = new EmailMessage.EmailAttachment
			//{
			//	AttachmentName = "Exception Details.txt",
			//	Attachment = Encoding.UTF8.GetBytes(exceptionDetail),
			//	MimeType =  MediaTypeNames.Text.Plain
			//};

			//mailMessage.Attachments.Add(stackTrace);

			//var notificationManager = new NotificationManager();

			//notificationManager.SendEmail(mailMessage);

			//var stat = new ExceptionStatistics { LastNotificationSentOn = DateTime.Now };

			//_exceptionStatistics[settings.NotificationEntryType] = stat;
        }

        /// <summary>
        /// Retrieves settings for the exception type requested
        /// </summary>
        /// <param name="exception">Exception for which setting are being requested</param>
        /// <returns>Object with the exception settings.</returns>
        //protected static ExceptionNotificationSettings FindSetting(Exception exception)
        //{
        //    var exceptionType = exception.GetType();

        //    var settingsToDictionary = ExceptionNotificationSettingsProvider.Settings.Select(e => e.Value).ToDictionary(k => k.NotificationEntryType);
            
        //    while (exceptionType != null)
        //    {
        //        if (!settingsToDictionary.ContainsKey(exceptionType))
        //        {
        //            exceptionType = exceptionType.BaseType;

        //            continue;
        //        }

        //        return settingsToDictionary[exceptionType];
        //    }

        //    WriteInformationToEventLog(string.Format("No setings have been defined for exception type: {0} or types it derives from. if you wish common settings to apply to all types, define settings for System.Exception", exception.GetType().Name));

        //    return null;
        //}

        /// <summary>
        /// Writes an exception to Unity event log if the task fails. It should never happen.
        /// </summary>
        /// <param name="exception"></param>
        protected void HandleExceptionFromLoggerAndNotification(Exception exception)
        {
            if (exception == null)
                return;

            WriteExceptionToEventLog(exception);
        }

        /// <summary>
        /// Builds the logger from DI container if it's configured and the logger is registered
        /// </summary>
        private bool TryBuildLogger(Exception exceptionToLog)
        {
            if (_logger != null)
                return true;

            if (ApplicationConfiguration.Instance.DependencyResolver == null)
            {
                WriteWarningToEventLog("No dependency resolver has been set up. Alghtough the exception has not been logged you could see the details of the exception in the S3ID event log");

                WriteExceptionToEventLog(exceptionToLog);

                return false;
            }

			var resolver = ApplicationConfiguration.Instance.DependencyResolver;

            
                _logger = resolver.ResolveAll<ILogProvider>()
					.FirstOrDefault();

                if (_logger == null)
                {
                    WriteWarningToEventLog("No implementation of ILogProvider has been found in DI container. Make sure it is registered in the container. The exception has not been processed. You could see the details of the exception in the Unity event log");

                    WriteExceptionToEventLog(exceptionToLog);

                    return false;
                }

            return true;
        }

        /// <summary>
        /// Logs the exception internal.
        /// </summary>
        /// <param name="exceptionToLog">The exception to log.</param>
        /// <param name="arguments">The arguments.</param>
        private void LogExceptionInternal(Exception exceptionToLog, object[] arguments)
        {
            Exception resultException;

            if (arguments != null)
            {
                foreach (var a in arguments)
                {
                    _logger.AddParameter(a.GetType().Name, a);
                }
            }

            var result = _logger.TryLogException(exceptionToLog, out resultException);

            if (!result)
                WriteWarningToEventLog("Processing the exception has failed. Check the Uinty event log for details");
        }

        /// <summary>
        /// Retrieves the application name from the application comfig file if it exists or default application name - Unity Application.
        /// </summary>
        /// <returns></returns>
        private static string GetApplicationName()
        {
            return ApplicationSettingProvider.ApplicationName;
        }

        /// <summary>
        /// Composes the text with exception details.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="exception"></param>
        private static void AddToExceptionDeails(StringBuilder stringBuilder, Exception exception)
        {
            stringBuilder.AppendFormat("Exception Type: {0}", exception.GetType().Name);

            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("Message: {0}", exception.Message);

            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("Stack: {0}", exception.StackTrace);

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("------------------------------------------------");
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_logger != null)
                    _logger.Dispose();
            }
        }

        #endregion
    }
}