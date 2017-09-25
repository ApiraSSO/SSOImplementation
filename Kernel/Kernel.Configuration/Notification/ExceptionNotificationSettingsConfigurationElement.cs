namespace Kernel.Configuration.Notification
{
	using System;
	using System.Configuration;
	using Kernel.Configuration;

    public class ExceptionNotificationSettingsConfigurationElement : AbstractConfigurationElement
    {
        [ConfigurationProperty("timeSpanInMinutes", IsRequired = true)]
        public String TimeSpan
        {
            get
            {
                return (String)this["timeSpanInMinutes"];
            }
            set
            {
                this["timeSpanInMinutes"] = value;
            }
        }

        [ConfigurationProperty("overrideRecipients", IsRequired = false)]
        public String OverrideRecipients
        {
            get
            {
                return (String)this["overrideRecipients"];
            }
            set
            {
                this["overrideRecipients"] = value;
            }
        }
    }
}