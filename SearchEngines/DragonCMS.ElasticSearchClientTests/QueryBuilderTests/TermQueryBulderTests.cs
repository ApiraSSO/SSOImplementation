using System;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Organisation;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.Dependencies;
using DragonCMS.Common.SearchEngine;
using DragonCMS.Common.SearchEngine.Query;
using DragonCMS.ElasticSearchClient.DocumentAPI;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClient.SearchAPI.Query;
using DragonCMS.ElasticSearchClient.SearchAPI.Query.ClauseBuilders;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.QueryBuilderTests
{
    [TestFixture]
    internal class TermQueryBulderTests
    {
        [Test]
        public void BuildBoolQueryTest_multi_records_found()
        {
            //ARRANGE
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");
            var person1Id = Guid.NewGuid();
            var person1 = PersonAggregateFactory.BuildPersonSearchModel(personId, "Jane", "Doe");

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<FieldContext>>(t => new TermClauseBuilder(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<NestedFieldContext>>(t => new NestedClauseBuilder(), Lifetime.Transient);
            //create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, indexId);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                documentclient.UpsertDocument(context);

                var context1 = new UpsertDocumentContext<EsPersonSearch>(person1Id) { Document = person1, IndexContext = indexContext };
                documentclient.UpsertDocument(context1);

                Thread.Sleep(1000);

                var queryContext = new QueryContext
                {
                    SearchFields = new[] 
                    {
                        new FieldContext { Path = "PersonName.LastName", Value = "Doe" }
                    },
                    
                };
                var queryBuilder = new BoolQueryBulder<EsPersonSearch>(dependencyResolver);
                var query = queryBuilder.BuildQuery(queryContext);

                var index = indexManager.BuildIndexName(indexContext);
                var searchResponse = client.Search<EsPersonSearch>(s => s.Query(query)
                .Index(index));
                var documents = searchResponse.Documents;
                
                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(2, documents.Count);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void BuildBoolQueryTest_with_nested_query()
        {
            //ARRANGE
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");
            var person1Id = Guid.NewGuid();
            var person1 = PersonAggregateFactory.BuildPersonSearchModel(personId, "Jane", "Doe");
            var dependencyResolver = new DependencyResolverMock();
            var organisationSearch = new EsOrganisationSearch { OrganisationName = "Organisation1" };
            person.Organisations.Add(organisationSearch);

           
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<FieldContext>>(t => new TermClauseBuilder(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<NestedFieldContext>>(t => new NestedClauseBuilder(), Lifetime.Transient);
            

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);

            //create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, indexId);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                documentclient.UpsertDocument(context);

                var context1 = new UpsertDocumentContext<EsPersonSearch>(person1Id) { Document = person1, IndexContext = indexContext };
                documentclient.UpsertDocument(context1);

                Thread.Sleep(1000);

                var queryContext = new QueryContext
                {
                    SearchFields = new[]
                    {
                        new NestedFieldContext { Path = "Organisations", Value = "Organisation1", PropertyName = "organisations.organisationName" },
                    },

                };
                var queryBuilder = new BoolQueryBulder<EsPersonSearch>(dependencyResolver);
                var query = queryBuilder.BuildQuery(queryContext);

                var index = indexManager.BuildIndexName(indexContext);
                var searchResponse = client.Search<EsPersonSearch>(s => s.Query(query)
                .Index(index));
                var documents = searchResponse.Documents;
                
                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, documents.Count);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void BuildBoolQueryTest_organisation_first_name_query()
        {
            //ARRANGE
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");
            var person1Id = Guid.NewGuid();
            var person1 = PersonAggregateFactory.BuildPersonSearchModel(personId, "Jane", "Doe");

            var organisationSearch = new EsOrganisationSearch { OrganisationName = "Organisation1" };
            person.Organisations.Add(organisationSearch);

            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<FieldContext>>(t => new TermClauseBuilder(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<NestedFieldContext>>(t => new NestedClauseBuilder(), Lifetime.Transient);
            //create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, indexId);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                documentclient.UpsertDocument(context);

                var context1 = new UpsertDocumentContext<EsPersonSearch>(person1Id) { Document = person1, IndexContext = indexContext };
                documentclient.UpsertDocument(context1);

                Thread.Sleep(1000);

                var queryContext = new QueryContext
                {
                    SearchFields = new[]
                    {
                        new NestedFieldContext { Path = "Organisations", Value = "Organisation1", PropertyName = "organisations.organisationName" },
                        new FieldContext { Path = "PersonName.FirstName", Value = "Jane" }
                    },

                };
                var queryBuilder = new BoolQueryBulder<EsPersonSearch>(dependencyResolver);
                var query = queryBuilder.BuildQuery(queryContext);

                var index = indexManager.BuildIndexName(indexContext);
                var searchResponse = client.Search<EsPersonSearch>(s => s.Query(query)
                .Index(index));
                var documents = searchResponse.Documents;

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(2, documents.Count);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }
    }
}