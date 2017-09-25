using System;
using System.Collections.Generic;
using System.Net;

namespace KafkaClient.Configs.ConfigurationPrviders.ProducerConfigurationProviders
{
    internal class ClientIdProvider : IConfigurationProvider
    {
        public ConfigurationScope ConfigurationScope
        {
            get
            {
                return ConfigurationScope.Producer;
            }
        }

        public KeyValuePair<string, object> GetConfigurationPair()
        {
            return new KeyValuePair<string, object>("client.id", String.Format("{0}/{1}",Dns.GetHostName(), "DragonCMS"));
        }
    }
}