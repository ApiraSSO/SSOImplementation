using System;
using System.Collections.Generic;
using System.Net;

namespace KafkaClient.Configs.ConfigurationPrviders.ConsumerConfigurationProviders
{
    internal class GroupIdProvider : IConfigurationProvider
    {
        public ConfigurationScope ConfigurationScope
        {
            get
            {
                return ConfigurationScope.Consumer;
            }
        }

        public KeyValuePair<string, object> GetConfigurationPair()
        {
            return new KeyValuePair<string, object>("group.id", String.Format("{0}/{1}",Dns.GetHostName(), "DragonCMS"));
        }
    }
}