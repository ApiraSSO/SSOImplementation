using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.Dependencies;
using DragonCMS.ElasticSearchClient.Connection;
using DragonCMS.ElasticSearchClient.Connection.Helpers;
using DragonCMS.ElasticSearchClient.Factories;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClient.IndexAPI.PersonIndexMappers;
using Nest;

namespace DragonCMS.ElasticSearchClientTests.MockDataFactories
{
    internal class SearchClientFactory
    {
        public static ElasticClient GetClient()
        {
            var clientFactory = SearchClientFactory.GetClientFactory();     
            var client = clientFactory.GetClient();
            return client;
        }

        public static IClientFactory GetClientFactory()
        {
            var connectionFactory = ConnectionSettingsFactoryHelper.GetHTTPConnection();
            var connectionPool = ConnectionSettingsFactoryHelper.GetSingleNodePool();
            var settingsProvider = new ConnectionSettingsProvider(connectionFactory, connectionPool);
            var connectionManager = new ConnectionManager(settingsProvider);
            var clientFactory = new ElasticClientFactory(connectionManager);
            return clientFactory;
        }

        public static void RegisterDependencies(IDependencyResolver resolver)
        {
            resolver.RegisterFactory<IndexMapper<EsPersonSearch>>(t => new PersonNestedPropertiesMapper(resolver), Lifetime.Transient);
            resolver.RegisterFactory<PropertyMapper<EsPersonSearch>>(t => new PersonOrganisationMapper(), Lifetime.Transient);
            resolver.RegisterFactory<PropertyMapper<EsPersonSearch>>(t => new PersonNameMapper(), Lifetime.Transient);
        }
    }
}