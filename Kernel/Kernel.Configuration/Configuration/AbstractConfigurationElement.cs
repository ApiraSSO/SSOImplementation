namespace Kernel.Configuration
{
	using System;
	using System.Configuration;

    /// <summary>
    /// Base class for ConfigurationElement
    /// </summary>
    public abstract class AbstractConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = false)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}