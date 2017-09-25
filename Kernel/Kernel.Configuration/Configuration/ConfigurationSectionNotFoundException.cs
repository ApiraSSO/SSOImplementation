using System;

namespace Kernel.Configuration
{
    [Serializable]
    public class ConfigurationSectionNotFoundException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionNotFoundException" /> class.
        /// </summary>
        public ConfigurationSectionNotFoundException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionNotFoundException" /> class.
        /// </summary>
        /// <param name="section">The section.</param>
        public ConfigurationSectionNotFoundException(string section)
            : base(section)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionNotFoundException" /> class.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConfigurationSectionNotFoundException(string section, Exception innerException)
            : base(section, innerException)
        {
        }
    }
}