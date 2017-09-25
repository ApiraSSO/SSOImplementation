using Kernel.Serialisation;

namespace Serialisation.CSV.SettingsProviders
{
    public abstract class SettingsProvider : ISerialisationSettingsProvider<CSVSettings>
    {
        /// <summary>
        /// Gets the settings internal.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns></returns>
        protected abstract CSVSettings GetSettingsInternal();

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        SerialisationSettings<CSVSettings> ISerialisationSettingsProvider<CSVSettings>.GetSettings()
        {
            var settings = this.GetSettingsInternal();
            return new SerialisationSettings<CSVSettings>(settings);
        }
    }
}
