using System;
using System.Configuration;
using Kernel.Configuration;

namespace Kernel.Configuration.Notification
{
    public class EmailConfigurationElement : AbstractConfigurationElement
    {
        [ConfigurationProperty("from", IsRequired = true)]
        public String From
        {
            get
            {
                return (String)this["from"];
            }
            set
            {
                this["from"] = value;
            }
        }

        [ConfigurationProperty("to", IsRequired = true)]
        public String To
        {
            get
            {
                return (String)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }

        [ConfigurationProperty("cc", IsRequired = false)]
        public String CC
        {
            get
            {
                return (String)this["cc"];
            }
            set
            {
                this["cc"] = value;
            }
        }

        [ConfigurationProperty("bcc", IsRequired = false)]
        public String BCC
        {
            get
            {
                return (String)this["bcc"];
            }
            set
            {
                this["bcc"] = value;
            }
        }

        [ConfigurationProperty("message", IsRequired = false)]
        public String Message
        {
            get
            {
                return (String)this["message"];
            }
            set
            {
                this["message"] = value;
            }
        }

        [ConfigurationProperty("subject", IsRequired = false)]
        public String Subject
        {
            get
            {
                return (String)this["subject"];
            }
            set
            {
                this["subject"] = value;
            }
        }
    }
}