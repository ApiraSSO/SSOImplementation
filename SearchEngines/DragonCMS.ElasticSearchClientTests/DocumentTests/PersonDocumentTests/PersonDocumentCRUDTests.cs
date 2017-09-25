using System;
using System.Threading;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.DDD;
using DragonCMS.Directory.Messages.Models.V1;
using DragonCMS.Directory.Messages.People.Events.V1;
using DragonCMS.Directory.Messages.Positions.Events.V1;
using DragonCMS.ElasticSearchClient.DocumentAPI;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using DragonCMS.ElasticSearchManagerAdapter.DocumentBuilders.Person;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.DocumentTests.PersonDocumentTests
{
    [TestFixture]
    public class PersonDocumentCRUDTests
    {
        [Test]
        public void CreatePersonSearchDocumentTest()
        {
            //ARRANGE
            //set up a person
            
            var dependencyResolver = new DependencyResolverMock();
            var id = Guid.NewGuid();
            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);

            //delete person index
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, id).ToLower(), Type = typeof(EsPersonSearch) };
            var deleteIndexResult = client.DeleteIndex(index);

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);

            var builder = new AddPersonContextBuilder();
            var personId = Guid.NewGuid();
            var ev = new NewPersonAdded(
                new AggregateId(personId),
                new CQRS.CorrelationId(personId), 
                null,
                1, 
                1, 
                1,
                new PersonName
                {
                    FirstName = "John",
                    LastName = "Doe"
                },
                null);

            var context = builder.BuildContext(ev);
            try
            {
                //create person document
                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                var document = client.Get<EsPersonSearch>(personId, d => d.Index(index));

                var personName = new PersonName
                {
                    FirstName = "John1",
                    LastName = "Doe1"
                };

                var builder1 = new AddPossitionContexttBuilder();
                var ev1 = new NewPositionAdded(
                    new AggregateId(personId),
                    null, 
                    new CQRS.CorrelationId(personId),
                    personId, 
                    Guid.NewGuid(), 
                    null,
                    1,
                    1, 
                    1,
                     personName,
                    "TestOrganisation");

                var newPossitionAdded = builder1.BuildContext(ev1);
                context = builder1.BuildContext(ev1);

                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                document = client.Get<EsPersonSearch>(personId, d => d.Index(index));

                var builder2 = new PersonNameEditedContextBuilder();
                var ev2 = new NameEdited(
                    new AggregateId(personId), 
                    1, 
                    new CQRS.CorrelationId(personId),
                    null,
                    personName,
                    1);


                context = builder2.BuildContext(ev2);


                documentclient.UpsertDocument(context);
                Thread.Sleep(1000);

                document = client.Get<EsPersonSearch>(personId, d => d.Index(index));
            }
            finally
            {
                client.DeleteIndex(index);
            }
        }

        //[Test]
        //public void UpdateDocumentTest()
        //{
        //    ARRANGE

        //    set up search client
        //    var dependencyResolver = new DependencyResolverMock();

        //    var client = SearchClientFactory.GetClient();
        //    var clientFactory = SearchClientFactory.GetClientFactory();
        //    SearchClientFactory.RegisterDependencies(dependencyResolver);

        //    delete existing index
        //    var indexId = Guid.NewGuid();
        //    var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };
        //    var deleteIndexResult = client.DeleteIndex(Indices.Index(index));

        //    set up document CRUD controller and create a mock document
        //    var indexManager = new IndexManager(dependencyResolver, clientFactory);
        //    var documentclient = new DocumentController(clientFactory, indexManager);

        //    dependencyResolver.RegisterFactory<IndexMapper<ParentTestClass>>(t => new TestIndexMapper(dependencyResolver), Lifetime.Transient);

        //    var testClass = new ParentTestClass();
        //    testClass.Children.Add(new ChildClass());

        //    var documentPath = new DocumentPath<ParentTestClass>(testClass)
        //        .Index(index)
        //        .Type(typeof(ParentTestClass));

        //    ACT
        //    try
        //    {
        //        var createRequest = new CreateRequest<ParentTestClass>(documentPath);
        //        var createResponse = client.Create(createRequest);
        //        Thread.Sleep(1000);
        //        var getResponse = client.Get<ParentTestClass>(documentPath);

        //        var loadedDoc = getResponse.Source;
        //        loadedDoc.Child = new ChildClass();
        //        var updateDocumentPath = new DocumentPath<ParentTestClass>(loadedDoc)
        //         .Index(index)
        //         .Type(typeof(ParentTestClass));
        //        var updateRequest = new UpdateRequest<ParentTestClass, ParentTestClass>(updateDocumentPath);
        //        updateRequest.Doc = loadedDoc;

        //        var updateResponse = client.Update<ParentTestClass>(updateRequest);
        //        var updateResponse = client.Update<ParentTestClass>(updateDocumentPath, d => d.Doc(loadedDoc));

        //        getResponse = client.Get<ParentTestClass>(updateDocumentPath);
        //        ASSERT
        //    }
        //    finally
        //    {
        //        deleteIndexResult = client.DeleteIndex(index);
        //    }
        //}
    }
}