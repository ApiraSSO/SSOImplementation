using Nest;

namespace ElasticSearchClient.Connection
{
    public interface IConnectionSettingsProvider
    {
        ConnectionSettings DefaultSettings();
    }
}