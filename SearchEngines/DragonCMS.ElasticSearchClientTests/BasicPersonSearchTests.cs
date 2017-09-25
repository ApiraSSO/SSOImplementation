using System;
using System.Linq;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.SearchEngine;
using DragonCMS.ElasticSearchClient.DocumentAPI;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests
{
    [TestFixture]
    public class BasicPersonSearchTests
    {
        [Test]
        public void SearchPerson_by_id_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Match(m => m.Field(g => g.Id)
                    .Query(personId.ToString())))
                    .Index(index)
                    .Explain());

                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(personId, searchResponse.Documents.First().Id);
                Assert.AreEqual("John", searchResponse.Documents.First().PersonName.FirstName);
                Assert.AreEqual("Doe", searchResponse.Documents.First().PersonName.LastName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_fore_name_term_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var index = indexManager.BuildIndexName(indexContext);
                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Term(m => m.Field(g => g.PersonName.FirstName)
                    .Value("John")))
                    .Index(index)
                    .Explain());
                
                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(personId, searchResponse.Documents.First().Id);
                Assert.AreEqual("John", searchResponse.Documents.First().PersonName.FirstName);
                Assert.AreEqual("Doe", searchResponse.Documents.First().PersonName.LastName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_fore_name_multi_records_wildchar_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

            var person1Id = Guid.NewGuid();
            var person1 = PersonAggregateFactory.BuildPersonSearchModel(person1Id, "John1", "Doe1");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);

                var context1 = new UpsertDocumentContext<EsPersonSearch>(person1Id) { Document = person1, IndexContext = indexContext };
                //create another person document
                documentclient.UpsertDocument(context1);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Wildcard(m => m.Field(g => g.PersonName.FirstName)
                    .Value("John*")))
                    .Index(index)
                    .Explain());

                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(2, searchResponse.Documents.Count());
                Assert.IsTrue(searchResponse.Documents.Any(x => x.Id == personId));
                Assert.IsTrue(searchResponse.Documents.Any(x => x.Id == person1Id));
                Assert.IsTrue(searchResponse.Documents.Any(x => x.PersonName.FirstName.Equals("John")));
                Assert.IsTrue(searchResponse.Documents.Any(x => x.PersonName.FirstName.Equals("John1")));
                Assert.IsTrue(searchResponse.Documents.Any(x => x.PersonName.LastName.Equals("Doe")));
                Assert.IsTrue(searchResponse.Documents.Any(x => x.PersonName.LastName.Equals("Doe1")));
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_fore_name_wildchar_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Wildcard(m => m.Field(g => g.PersonName.FirstName)
                    .Value("Jo*n")))
                    .Index(index)
                    .Explain());


                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(personId, searchResponse.Documents.First().Id);
                Assert.AreEqual("John", searchResponse.Documents.First().PersonName.FirstName);
                Assert.AreEqual("Doe", searchResponse.Documents.First().PersonName.LastName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_last_name_term_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Term(m => m.Field(g => g.PersonName.LastName)
                    .Value("Doe")))
                    .Index(index)
                    .Explain());


                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(personId, searchResponse.Documents.First().Id);
                Assert.AreEqual("John", searchResponse.Documents.First().PersonName.FirstName);
                Assert.AreEqual("Doe", searchResponse.Documents.First().PersonName.LastName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_last_name_wildchar_Test()
        {
            //ARRANGE
            //set up a person
            var personId = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s => s.Query(q =>
                    q.Wildcard(m => m.Field(g => g.PersonName.LastName)
                    .Value("D*e")))
                    .Index(index)
                    .Explain());


                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(personId, searchResponse.Documents.First().Id);
                Assert.AreEqual("John", searchResponse.Documents.First().PersonName.FirstName);
                Assert.AreEqual("Doe", searchResponse.Documents.First().PersonName.LastName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }

        [Test]
        public void SearchPerson_by_organisation_Test()
        {
            //ARRANGE
            var personId = Guid.NewGuid();
            var organisationId1 = Guid.NewGuid();
            var organisationId2 = Guid.NewGuid();
            var person = PersonAggregateFactory.BuildPersonSearchModel(personId, "John", "Doe");
            PersonAggregateFactory.AddPersonOrganisation(person, organisationId1, "TestOrganisation1");
            PersonAggregateFactory.AddPersonOrganisation(person, organisationId2, "TestOrganisation2");

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
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            try
            {
                var index = indexManager.BuildIndexName(indexContext);
                //create person document
                var context = new UpsertDocumentContext<EsPersonSearch>(personId) { Document = person, IndexContext = indexContext };
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var searchResponse = client
                    .Search<EsPersonSearch>(s =>
                    s.Query(q =>
                            q.Nested(nq =>
                            nq.Path(p => p.Organisations)
                            .Query(oq =>
                                      oq.Match(m => m.Field(g => g.Organisations.First().OrganisationName)
                                      .Query("TestOrganisation1")))))
                    .Index(index));

                var allRecords = client
                    .Search<EsPersonSearch>(q => q
                    .Index(index));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(person.Id, searchResponse.Documents.First().Id);
                Assert.AreEqual(2, searchResponse.Documents.First().Organisations.Count());
                Assert.AreEqual("TestOrganisation1", searchResponse.Documents.First().Organisations.First().OrganisationName);
            }
            finally
            {
                indexManager.DeleteIndex(indexContext);
            }
        }
    }
}