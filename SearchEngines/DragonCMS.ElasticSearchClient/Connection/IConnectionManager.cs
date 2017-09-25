using Nest;

namespace ElasticSearchClient.Connection
{
    public interface IConnectionManager
    {
        ConnectionSettings GetConnectionSettings();
    }
}