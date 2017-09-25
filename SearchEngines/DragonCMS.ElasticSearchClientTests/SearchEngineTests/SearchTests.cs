using System;
using System.Linq;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person.SearchResultModels;
using DragonCMS.Common.Dependencies;
using DragonCMS.Common.SearchEngine;
using DragonCMS.Common.SearchEngine.Query;
using DragonCMS.ElasticSearchClient.DocumentAPI;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClient.SearchAPI;
using DragonCMS.ElasticSearchClient.SearchAPI.Query;
using DragonCMS.ElasticSearchClient.SearchAPI.Query.ClauseBuilders;
using DragonCMS.ElasticSearchClient.SearchAPI.ResultProjectors;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.SearchEngineTests
{
    [TestFixture]
    internal class SearchTests
    {
        [Test]
        public void BuildBoolMustQueryTest_multi_records_found()
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
            
            //create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, indexId);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            dependencyResolver.RegisterFactory<ISearchClauseBuilder<EsPersonSearch>>((t) => new SearchClauseBuilder<EsPersonSearch>(new BoolQueryBulder<EsPersonSearch>(dependencyResolver), new SortClauseBuilder<EsPersonSearch>(), indexManager), Lifetime.Transient);
            dependencyResolver.RegisterFactory<ResultProjector<EsPersonSearch, QmPersonSearchResult>>(t => new PersonResultProjector(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<FieldContext>>(t => new TermClauseBuilder(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IQueryClauseBuilder<NestedFieldContext>>(t => new NestedClauseBuilder(), Lifetime.Transient);
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            var searchEngine = new SearchEngine(clientFactory, dependencyResolver, responseHandler);

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
                    IndexContext = new IndexContext<EsPersonSearch> { IndexName = indexName },

                };
                queryContext.SortContext.Fields.Add(new SortField { Path = "PersonName.FirstName" });

                var searchResponse = searchEngine.Search<EsPersonSearch, QmPersonSearchResult>(queryContext).Result;
               
                var documents = searchResponse.Entities;

                //ASSERT
                
                Assert.AreEqual(2, documents.Count());
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }
    }
}