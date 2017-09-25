using System.Collections.Generic;

namespace KafkaClient.Configs.ConfigurationPrviders
{
    public interface IConfigurationProvider
    {
        ConfigurationScope ConfigurationScope { get; }
        KeyValuePair<string, object> GetConfigurationPair();
    }
}