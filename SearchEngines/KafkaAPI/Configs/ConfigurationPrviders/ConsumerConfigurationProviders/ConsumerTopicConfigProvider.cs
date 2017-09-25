using System.Collections.Generic;

namespace KafkaClient.Configs.ConfigurationPrviders.ConsumerConfigurationProviders
{
    internal class ConsumerTopicConfigProvider : TopicConfigProvider
    {
        public override ConfigurationScope ConfigurationScope
        {
            get
            {
                return ConfigurationScope.Consumer;
            }
        }

        protected override KeyValuePair<string, object> GetConfigurationInternal()
        {
            return new KeyValuePair<string, object>("default.topic.config", new Dictionary<string, object>
                    {
                        { "auto.offset.reset", "smallest" }
                    });
        }
    }
}