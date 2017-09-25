using System;
using Elasticsearch.Net;

namespace ElasticSearchClient.Connection.Helpers
{
    internal class ConnectionSettingsFactoryHelper
    {
        internal static Func<IConnection> GetHTTPConnection()
        {
            return () => (new HttpConnection());
        }

        internal static Func<IConnection> GetInMemmoryConnection()
        {
            return () => (new InMemoryConnection());
        }

        internal static Func<IConnectionPool> GetSingleNodePool()
        {
            return () => {
                //get the nodes from config etc. and implement settings provider
                //this is the default local service when installed.
                var node = new Uri("http://localhost:9200");
                return new SingleNodeConnectionPool(node);
            };
        }
    }
}