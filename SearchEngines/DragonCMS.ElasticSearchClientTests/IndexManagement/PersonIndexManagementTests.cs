using System;
using System.Linq;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.SearchEngine;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.IndexManagement
{
    [TestFixture]
    internal class PersonIndexManagementTests
    {
        [Test]
        public void MapPersonNamePropertyTests()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            
            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);
            
            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);
            var index = indexManager.GetIndex(indexContext).Result;
            //ACT

            try
            {
                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();

                var mappings = first.Value.Mappings.Select(x => x.Value)
                    .SelectMany(x => x.Properties, (x, y) =>
                    {
                        return new { y.Key, y.Value.Name, y.Value.Type,  Properties = ((ObjectProperty)y.Value).Properties };
                    });
                var nameProperty = mappings.First(x => x.Name.Name.Equals("personName", StringComparison.OrdinalIgnoreCase));
                var firstNameProperty = nameProperty.Properties.First(x => x.Key.Name.Equals("firstName", StringComparison.OrdinalIgnoreCase));
                var lastNameProperty = nameProperty.Properties.First(x => x.Key.Name.Equals("lastName", StringComparison.OrdinalIgnoreCase));
                
                //ASSERT person name
                Assert.IsTrue(indexResponse.IsValid);
                Assert.AreEqual(1, indices.Count);
                Assert.AreEqual("object", nameProperty.Type.Name);

                //ASSERT person first name
                Assert.AreEqual("keyword", firstNameProperty.Value.Type.Name);

                //ASSERT person last name
                Assert.AreEqual("keyword", lastNameProperty.Value.Type.Name);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        [Test]
        public void MapPersonOrganisationPropertyTests()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);

            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);
            var index = indexManager.GetIndex(indexContext).Result;
            //ACT

            try
            {
                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();

                var mappings = first.Value.Mappings.Select(x => x.Value)
                    .SelectMany(x => x.Properties, (x, y) =>
                    {
                        return new { y.Key, y.Value.Name, y.Value.Type, Properties = ((ObjectProperty)y.Value).Properties };
                    });
                
                var organisationProperty = mappings.First(x => x.Name.Name.Equals("organisations", StringComparison.OrdinalIgnoreCase));
                //ASSERT
                Assert.IsTrue(indexResponse.IsValid);
                Assert.AreEqual(1, indices.Count);
                Assert.AreEqual("nested", organisationProperty.Type.Name);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }
    }
}