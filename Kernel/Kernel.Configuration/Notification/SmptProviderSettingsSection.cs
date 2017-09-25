using System.Configuration;

namespace Kernel.Configuration.Notification
{
    public class SmptProviderSettingsSection : ConfigurationSectionBase
    {
        [ConfigurationProperty("smptSettings", IsDefaultCollection = false)]
        public SmptSettingsCollections SmptSettings
        {
            get
            {
                SmptSettingsCollections smptSettings =
                (SmptSettingsCollections)base["smptSettings"];
                return smptSettings;
            }
        }
    }
}