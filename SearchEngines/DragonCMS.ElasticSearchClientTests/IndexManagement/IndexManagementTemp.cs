using System;
using System.Linq;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
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
    [Ignore("!!!!Infrastrcuture methods. Not to be run.!!!!")]
    internal class IndexManagementTemp
    {
        [Test]
        public void GetIndexState()
        {
            //ARRANGE
            //var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}", typeof(EsPersonSearch).Name).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext<ParentTestClass>(indexName);
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            var indexDescriptor = new CreateIndexDescriptor(index).Mappings(map => map.Map<EsPersonSearch>(m => m.AutoMap()));
            
            //ACT

            try
            {
                var template = client.GetIndexTemplate(d => d.Name("personsearch"));
                //var documents = client.Search<EsPersonSearch>(s =>
                //s.Query(q => q.MatchAll())
                //.Index(index));

                // var document = client.Search<EsPersonSearch>(s =>
                //s.Query(q => q.Match(m => 
                //                    m.Field(f => f.PersonName.LastName)
                //.Query("Doe")))
                //.Index(index));
                //var allIndices = client.GetIndex(Indices.All);
                var indexState = indexManager.GetIndexState(indexContext);
                var providedName = indexState.Settings["index.provided_name"];
                var propertiesMapped = indexState.Mappings.SelectMany(x => x.Value.Properties);
                //ASSERT
                Assert.AreEqual(indexName, providedName);
            }
            finally
            {
                //client.DeleteIndex(index);
            }
        }

        [Test]
        public void Create_person_search_index()
        {
            //ARRANGE
            //var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            var indexName = String.Format("{0}", typeof(EsPersonSearch).Name).ToLower();
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var indexContext = new IndexContext<ParentTestClass>(indexName);
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            var indexDescriptor = new CreateIndexDescriptor(index).Mappings(map => map.Map<EsPersonSearch>(m => m.AutoMap()));
            //        .Mappings(map => map.Map<ParentTestClass>(m =>
            //        m.Properties(p => p.Keyword(d => d.Name(n => n.Email))).AutoMap()));
            //var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                client.CreateIndex(indexDescriptor);
                //indexManager.BuildIndex(index).Wait();
                //Thread.Sleep(1000);
                //var d = client.Get<EsPersonSearch>(new DocumentPath<EsPersonSearch>(new EsPersonSearch { Id = Guid.Parse("a8de6650-236e-46ff-b3b9-b0bb24f0a38b") })
                //    .Index(index));
                var documents = client.Search<EsPersonSearch>(s =>
                s.Query(q => q.MatchAll())
                .Index(index));

                var document = client.Search<EsPersonSearch>(s =>
               s.Query(q => q.Match(m =>
                                   m.Field(f => f.PersonName.LastName)
               .Query("Doe")))
               .Index(index));

                var indexState = indexManager.GetIndexState(indexContext);
                var providedName = indexState.Settings["index.provided_name"];
                var propertiesMapped = indexState.Mappings.SelectMany(x => x.Value.Properties);
                //ASSERT
                Assert.AreEqual(indexName, providedName);
            }
            finally
            {
                //client.DeleteIndex(index);
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
        [Ignore("!!!!!!!!!DANGEROUS. DO NOT RUN THIS ONE.!!!!!!!!!!!!")]
        public void Delete_all_indices()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create an unique index
            //var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            //var responseHandler = new ResponseHandler();
            //var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            //var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            //var index = indexManager.BuildIndexName(indexContext);
            //ACT

            try
            {
                client.DeleteIndex(Indices.All);
                //Thread.Sleep(1000);
                //var indexReqiest = new GetIndexRequest(index);
                //var indexResponse = client.GetIndex(indexReqiest);
                
                ////delete the index
                //indexManager.DeleteIndex(indexContext);

                //var indexResponseAfterDelete = client.GetIndex(indexReqiest);
                ////ASSERT
                //Assert.IsTrue(indexResponse.IsValid);
                //Assert.IsFalse(indexResponseAfterDelete.IsValid);
            }
            finally
            {
                //client.DeleteIndex(index);
            }
        }
    }
}