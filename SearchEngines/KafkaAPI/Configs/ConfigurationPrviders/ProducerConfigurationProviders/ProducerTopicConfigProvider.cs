using System.Collections.Generic;

namespace KafkaClient.Configs.ConfigurationPrviders.ProducerConfigurationProviders
{
    internal class ProducerTopicConfigProvider : TopicConfigProvider
    {
        public override ConfigurationScope ConfigurationScope
        {
            get
            {
                return ConfigurationScope.Producer;
            }
        }

        protected override KeyValuePair<string, object> GetConfigurationInternal()
        {
            return new KeyValuePair<string, object>("default.topic.config", new Dictionary<string, object>
                    {
                        { "acks", "all" },
                    });
        }
    }
}