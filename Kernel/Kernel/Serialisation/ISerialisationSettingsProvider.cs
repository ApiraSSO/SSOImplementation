namespace Kernel.Serialisation
{
    public interface ISerialisationSettingsProvider<TSettings>
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        SerialisationSettings<TSettings> GetSettings();
    }
}