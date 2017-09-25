using System;

namespace Kernel.Configuration
{
    [Serializable]
    public class ConfigurationEntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEntityNotFound"/> class.
        /// </summary>
        public ConfigurationEntityNotFoundException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEntityNotFound"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="section">The section.</param>
        public ConfigurationEntityNotFoundException(string entity, string section)
            : base(string.Format("Section: {0}, message: {1}"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEntityNotFound"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="section">The section.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConfigurationEntityNotFoundException(string entity, string section, Exception innerException)
			: base(string.Format("Section: {0}, message: {1}"), innerException)
        {
        }
    }
}