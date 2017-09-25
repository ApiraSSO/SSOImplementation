using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using ElasticSearchClient.Connection.Helpers;
using Kernel.DependancyResolver;
using Shared.Initialisation;

namespace ElasticSearchClient.Initialisation
{
    public class ClientInitialiser : Initialiser
    {
        public override byte Order
        {
            get
            {
                return 0;
            }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterFactory<Func<IConnection>>(ConnectionSettingsFactoryHelper.GetHTTPConnection, Lifetime.Singleton);
            dependencyResolver.RegisterFactory<Func<IConnectionPool>>(ConnectionSettingsFactoryHelper.GetSingleNodePool, Lifetime.Singleton);
            return Task.CompletedTask;
        }
    }
}