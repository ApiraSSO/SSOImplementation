using System.Configuration;

namespace Kernel.Configuration
{
    /// <summary>
    /// Base class for ConfigurationSection
    /// </summary>
    public abstract class ConfigurationSectionBase : ConfigurationSection
    {
        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static T GetSection<T>(string sectionName) where T : class
        {
            return GetSection<T>(sectionName, true);
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static T GetSection<T>(string sectionName, bool throwException) where T : class
        {
            T section = (T)ConfigurationManager.GetSection(sectionName);

            if (section == null && throwException)
            {
                throw new ConfigurationSectionNotFoundException(sectionName);
            }

            return section;
        }
    }
}