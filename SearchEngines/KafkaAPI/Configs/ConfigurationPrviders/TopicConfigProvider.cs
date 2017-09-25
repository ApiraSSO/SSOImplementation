using System.Collections.Generic;

namespace KafkaClient.Configs.ConfigurationPrviders
{
    internal abstract class TopicConfigProvider : IConfigurationProvider
    {
        public abstract ConfigurationScope ConfigurationScope { get; }

        public KeyValuePair<string, object> GetConfigurationPair()
        {
            return this.GetConfigurationInternal();
        }

        protected abstract KeyValuePair<string, object> GetConfigurationInternal();
    }
}