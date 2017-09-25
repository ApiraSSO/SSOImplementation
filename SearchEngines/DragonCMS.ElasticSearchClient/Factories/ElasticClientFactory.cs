using ElasticSearchClient.Connection;
using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.Factories
{
    internal class ElasticClientFactory : IClientFactory, IAutoRegisterAsTransient
    {
        IConnectionManager _connectionManager;
        public ElasticClientFactory(IConnectionManager connectionManager)
        {
            this._connectionManager = connectionManager;
        }
        public ElasticClient GetClient()
        {
            var connectionSettings = this._connectionManager.GetConnectionSettings();
            var client = new ElasticClient(connectionSettings);
            return client;
        }
    }
}