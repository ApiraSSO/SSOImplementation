using Kernel.Serialisation;
using Newtonsoft.Json;

namespace Serialisation.JSON.SettingsProviders
{
    public abstract class SettingsProvider : ISerialisationSettingsProvider<JsonSerializerSettings>
    {
        /// <summary>
        /// Gets the settings internal.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns></returns>
        protected abstract JsonSerializerSettings GetSettingsInternal();

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        SerialisationSettings<JsonSerializerSettings> ISerialisationSettingsProvider<JsonSerializerSettings>.GetSettings()
        {
            var settings = this.GetSettingsInternal();
            return new SerialisationSettings<JsonSerializerSettings>(settings);
        }
    }
}
