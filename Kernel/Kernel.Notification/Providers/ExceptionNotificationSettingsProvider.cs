namespace Kernel.Notification.Providers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Kernel.Configuration;
    using Kernel.Configuration.Notification;
    using Kernel.Logging;
    using Kernel.Notification.Exceptions;
    using Kernel.Reflection;

    public class ExceptionNotificationSettingsProvider : SettingsProviderBase
    {
        private static Exception _error;

        private static bool _isValid;

        private class TypeComparer : IEqualityComparer<Type>
        {

            public bool Equals(Type x, Type y)
            {
                return x.FullName.Equals(y.FullName);
            }

            public int GetHashCode(Type obj)
            {
                return typeof(Type).GetHashCode();
            }
        }

        private static Task _initTask;

        private static EmailMessage _emailMessage;

        private static IList<ValidationResult> _validationResult;

        private static IDictionary<string, ExceptionNotificationSettings> _settings;

        public static EmailMessage EmailMessageClone
        {
            get
            {
                return (EmailMessage)_emailMessage.Clone();
            }
        }

        public static IDictionary<string, ExceptionNotificationSettings> Settings 
        {
            get
            {
                return _settings;
            }
        }

        public override IList<ValidationResult> ValidationResult
        {
            get
            {
                return _validationResult;
            }
        }
        
        public override bool IsValid
        {
            get { return _isValid; }
        }

        public bool IsInitialised { get { return _initTask.IsCompleted; } }

        public override Exception Error
        {
            get { return _error; }
        }

        static ExceptionNotificationSettingsProvider()
        {
            _initTask = InitInternal();
        }

        public void WaitToInitialise()
        {
            _initTask.Wait();
        }

        private async static Task InitInternal()
        {
            try
            {
                if (_validationResult == null)
                    _validationResult = new List<ValidationResult>();
                
                var section = ConfigurationSectionBase.GetSection<ExceptionNotificationSettingsSection>("exceptionNotificationSettingsSection");

                if (section == null)
                    return;

                _emailMessage = new EmailMessage();

                _settings = new Dictionary<string, ExceptionNotificationSettings>();

                await Task.Factory.StartNew(() =>
                    {
                        var types = ReflectionHelper.GetAllTypes(t => typeof(Exception).IsAssignableFrom(t))
                            .Distinct(new TypeComparer());

                        PopulateEmaiMessage(section.EmailConfiguration);

                        var timer = new Stopwatch();

                        LoggerManager.WriteInformationToEventLog("Loading all exceptions...");

                        timer.Start();

                        var dict = types.ToDictionary(k => k.FullName);

                        timer.Stop();

                        LoggerManager.WriteInformationToEventLog(string.Format("Loading all exceptions took {0}", timer.Elapsed));


                        foreach (ExceptionNotificationSettingsConfigurationElement setting in section.ExceptionNotificationSettings)
                        {
                            if (!dict.ContainsKey(setting.Name) || _settings.ContainsKey(setting.Name))
                                continue;

                            double minutes;

                            if (!ReflectionHelper.TryParseOrDefault<double>(setting.TimeSpan, out minutes))
                            {
                                _validationResult.Add(new ValidationResult(string.Format("Configuration setting TimeSpan for {0} is not numerical. Value is {1}.", setting.Name, setting.TimeSpan)));

                                LoggerManager.WriteWarningToEventLog(string.Format("Configuration setting TimeSpan for {0} is not numerical. Value is {1}.", setting.Name, setting.TimeSpan));

                                continue;
                            }

                            var newSettings = new ExceptionNotificationSettings
                            {
                                NotificationEntryType = dict[setting.Name],
                                TimeSpan = TimeSpan.FromMinutes(minutes),
                                To = setting.OverrideRecipients.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            };

                            if (!IsSettingsValid(newSettings, _validationResult))
                                continue;

                            _settings[setting.Name] = newSettings;
                        }
                    });
            }
            catch (Exception ex)
            {
                _error = ex;

                LoggerManager.WriteExceptionToEventLog(ex);
            }
            finally
            {
                _isValid = _error == null && _validationResult.Count == 0;
            }
        }

        private static bool IsSettingsValid(ExceptionNotificationSettings settings, IList<ValidationResult> validationResult)
        {
            return Validator.TryValidateObject(settings, new ValidationContext(settings, null), validationResult);
        }

        private static void PopulateEmaiMessage(EmailConfigurationElement emailConfiguration)
        {
            if (!string.IsNullOrWhiteSpace(emailConfiguration.From))
                _emailMessage.From = emailConfiguration.From;
            
            if (!string.IsNullOrWhiteSpace(emailConfiguration.BCC))
                _emailMessage.Bcc = emailConfiguration.BCC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!string.IsNullOrWhiteSpace(emailConfiguration.CC))
                _emailMessage.Cc = emailConfiguration.CC.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!string.IsNullOrWhiteSpace(emailConfiguration.To))
                _emailMessage.To = emailConfiguration.To.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (!string.IsNullOrWhiteSpace(emailConfiguration.Message))
                _emailMessage.Message = emailConfiguration.Message;

            if (!string.IsNullOrWhiteSpace(emailConfiguration.Subject))
                _emailMessage.Subject = emailConfiguration.Subject;
        }
    }
}