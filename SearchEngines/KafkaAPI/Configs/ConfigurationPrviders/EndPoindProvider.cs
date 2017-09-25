using System.Collections.Generic;

namespace KafkaClient.Configs.ConfigurationPrviders
{
    internal class EndPoindProvider : IConfigurationProvider
    {
        public ConfigurationScope ConfigurationScope
        {
            get
            {
                return ConfigurationScope.Consumer | ConfigurationScope.Producer;
            }
        }

        public KeyValuePair<string, object> GetConfigurationPair()
        {
            return new KeyValuePair<string, object>("bootstrap.servers", "localhost:9092,localhost:9092");
        }
    }
}