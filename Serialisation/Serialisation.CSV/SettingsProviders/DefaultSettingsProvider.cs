namespace Serialisation.CSV.SettingsProviders
{
    public class DefaultSettingsProvider : SettingsProvider
    {
        protected override CSVSettings GetSettingsInternal()
        {
            return new CSVSettings();
        }
    }
}