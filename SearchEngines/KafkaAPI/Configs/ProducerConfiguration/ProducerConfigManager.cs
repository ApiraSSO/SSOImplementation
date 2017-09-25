using System;
using System.Collections.Generic;
using System.Linq;
using KafkaClient.Configs.ConfigurationPrviders;
using Kernel.DependancyResolver;

namespace KafkaClient.Configs.ProducerConfiguration
{
    internal class ProducerConfigManager
    {
        private readonly IDependencyResolver _resolver;

        public ProducerConfigManager(IDependencyResolver resolver)
        {
            this._resolver = resolver;
        }
        public IDictionary<string, object> GetConfiguration(Func<IConfigurationProvider, bool> predicate)
        {
            var providers = this._resolver.ResolveAll<IConfigurationProvider>()
                .Where(predicate);

            return providers.Aggregate(new Dictionary<string, object>(), (d, next) => 
            {
                var config = next.GetConfigurationPair();
                d.Add(config.Key, config.Value);
                return d;
            });
        }
    }
}