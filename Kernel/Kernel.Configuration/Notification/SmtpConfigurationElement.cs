using System;
using System.Configuration;

namespace Kernel.Configuration.Notification
{
    public class SmtpConfigurationElement : AbstractConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get
            {
                return (String)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
