using System;
using System.Linq;
using System.Threading;
using DragonCMS.Common.Dependencies;
using DragonCMS.Common.SearchEngine;
using DragonCMS.Common.SearchEngine.Query;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClient.SearchAPI.Query.ClauseBuilders;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.IndexManagement
{
    [TestFixture]
    internal class IndexManagementTests
    {
        [Test]
        public void CreateIndex_if_not_exist_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            
            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                indexManager.BuildIndex(index).Wait();
                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();
                var providedName = first.Value.Settings["index.provided_name"];
                //ASSERT
                Assert.IsTrue(indexResponse.IsValid);
                Assert.AreEqual(1, indices.Count);
                Assert.AreEqual(indexName, providedName);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        [Test]
        public void CreateIndex_if_exist_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                indexManager.BuildIndex(index).Wait();
                Thread.Sleep(1000);
                indexManager.BuildIndex(index).Wait();
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();
                var providedName = first.Value.Settings["index.provided_name"];
                //ASSERT
                Assert.IsTrue(indexResponse.IsValid);
                Assert.AreEqual(1, indices.Count);
                Assert.AreEqual(indexName, providedName);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        [Test]
        public void BuildIndexName_name_type_provided_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id);
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            
            //ACT
            
            var index = indexManager.BuildIndexName(indexContext);

            //ASSERT
            Assert.AreEqual(indexName.ToLower(), index.Name);
            Assert.AreEqual(typeof(ParentTestClass), index.Type);
        }

        [Test]
        public void BuildIndexName_name_no_name_provided_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(ParentTestClass));

            //ACT

            var index = indexManager.BuildIndexName(indexContext);

            //ASSERT
            Assert.AreEqual(typeof(ParentTestClass).Name.ToLower(), index.Name);
            Assert.AreEqual(typeof(ParentTestClass), index.Type);
        }

        [Test]
        public void BuildIndexName_name_no_type_provided_test()
        {
            //ARRANGE
            
            //ACT
            
            //ASSERT
            Assert.Throws<ArgumentNullException>(() => new IndexContext(null));
        }

        [Test]
        public void GetIndexState_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext<ParentTestClass>(indexName);
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.Properties(p => p.Keyword(d => d.Name(n => n.Email))).AutoMap()));
            //var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                client.CreateIndex(indexDescriptor);
                //indexManager.BuildIndex(index).Wait();
                Thread.Sleep(1000);
               
                var indexState = indexManager.GetIndexState(indexContext);
                var providedName = indexState.Settings["index.provided_name"];
                var propertiesMapped = indexState.Mappings.SelectMany(x => x.Value.Properties);
                //ASSERT
                Assert.AreEqual(indexName, providedName);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        [Test]
        public void GetPropertyMetaData_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            dependencyResolver.RegisterFactory<QueryClauseBuilder<FieldContext>>(t => new TermClauseBuilder(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<QueryClauseBuilder<NestedFieldContext>>(t => new NestedClauseBuilder(), Lifetime.Transient);
            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext<ParentTestClass>(indexName);
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.Properties(p => p.Keyword(d => d.Name(n => n.Email))).AutoMap()));
            
            //ACT

            try
            {
                client.CreateIndex(indexDescriptor);
               
                Thread.Sleep(1000);

                //var indexState = client.GetIndex(index);
                var emailPropertyMetaData = indexManager.GetPropertyMetaData<ParentTestClass>(indexContext, x => x.Email);
                var childPropertyMetaData = indexManager.GetPropertyMetaData<ParentTestClass>(indexContext, x => x.Child);

                //ASSERT
                Assert.AreEqual("keyword", emailPropertyMetaData.Type.Name);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        [Test]
        public void GetNestedPropertyMetaData_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext<ParentTestClass>(indexName);
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.Properties(p => p.Keyword(d => d.Name(n => n.Email))).AutoMap()));

            //ACT

            try
            {
                client.CreateIndex(indexDescriptor);

                Thread.Sleep(1000);

                //var childPropertyMetaData = indexManager.GetPropertyMetaData<ParentTestClass>(indexContext, x => x.Child.Name);
                var childPropertyMetaData = indexManager.GetPropertyMetaData<ParentTestClass>(indexContext, "ParentTestClass.Child.Name");
                //ASSERT
                Assert.AreEqual("text", childPropertyMetaData.Type.Name);
            }
            finally
            {
                client.DeleteIndex(index);
                //client.DeleteIndex(Indices.All);
            }
        }

        [Test]
        public void DeletIndex_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                client.CreateIndex(index);
                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                
                //delete the index
                indexManager.DeleteIndex(indexContext);

                var indexResponseAfterDelete = client.GetIndex(indexReqiest);
                //ASSERT
                Assert.IsTrue(indexResponse.IsValid);
                Assert.IsFalse(indexResponseAfterDelete.IsValid);
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }
    }
}