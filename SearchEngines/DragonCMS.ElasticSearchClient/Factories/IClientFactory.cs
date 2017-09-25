using Nest;

namespace ElasticSearchClient.Factories
{
    public interface IClientFactory
    {
        ElasticClient GetClient();
    }
}