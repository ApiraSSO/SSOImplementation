using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests
{
    [TestFixture]
    public class DocumentControllerTests
    {
        //[Test]
        //public async Task CreatePersonDocumentTest()
        //{
        //    //ARRANGE
        //    //set up a person
        //    var userId = new UserId(Guid.NewGuid());
        //    var aggregateId = new AggregateId(Guid.NewGuid());
        //    var correlationId = new CorrelationId(Guid.NewGuid());
        //    var userActionDate = new UserActionDate(userId, DateTimeOffset.Now);

        //    var mockEvent = new CreatePersonMockEvent(
        //        aggregateId, correlationId, userActionDate
        //        , 1, 1, 1, String.Empty, "Jane", String.Empty, "Doe", String.Empty, userActionDate);
        //    var person = new Person();
        //    await StaticType<Product>.Preload(() => Task.FromResult(new List<Product> { new Product(1, "TestProduct", 1) }));
        //    await StaticType<PersonType>.Preload(() => Task.FromResult(new List<PersonType> { new PersonType(1, "TestPersonType") }));
        //    person.Apply(mockEvent);

        //    //set up search client
        //    var dependencyResolver = new DependencyResolverMock();
        //    var connectionFactory = ConnectionSettingsFactoryHelper.GetHTTPConnection();
        //    var connectionPool = ConnectionSettingsFactoryHelper.GetSingleNodePool();
        //    var settingsProvider = new ConnectionSettingsProvider(connectionFactory, connectionPool);
        //    var connectionManager = new ConnectionManager(settingsProvider);
        //    var clientFactory = new ElasticClientFactory(connectionManager);
        //    var indexManager = new IndexManager(dependencyResolver, clientFactory);
        //    var documentclient = new DocumentController(clientFactory, indexManager);

        //    //to call the server to assert... there should be a test server.!!!!!
        //    var client = clientFactory.GetClient();
        //    //ACT
        //    documentclient.UpsertDocument(person);
        //    //ASSERT
        //    var index = new IndexName { Name = typeof(Person).Name.ToLower(), Type = typeof(Person) };
        //    var getRequest = new GetRequest(index, TypeName.Create<Person>(), person.Id);
        //    var documentResponse = client.Get<Person>(getRequest);
        //    //var p = documentResponse.Source;
        //}

        //[Test]
        //public async Task CreatePersonWithCollectionItemDocumentTest()
        //{
        //    //ARRANGE
        //    //set up a person
        //    var userId = new UserId(Guid.NewGuid());
        //    var aggregateId = new AggregateId(Guid.NewGuid());
        //    var correlationId = new CorrelationId(Guid.NewGuid());
        //    var userActionDate = new UserActionDate(userId, DateTimeOffset.Now);

        //    var mockEvent = new CreatePersonMockEvent(
        //        aggregateId, correlationId, userActionDate
        //        , 1, 1, 1, String.Empty, "Jane1", String.Empty, "Doe1", String.Empty, userActionDate);
        //    var person = new Person();
        //    await StaticType<Product>.Preload(() => Task.FromResult(new List<Product> { new Product(1, "TestProduct", 1) }));
        //    await StaticType<PersonType>.Preload(() => Task.FromResult(new List<PersonType> { new PersonType(1, "TestPersonType") }));
        //    await StaticType<ContactNumberType>.Preload(() => Task.FromResult(new List<ContactNumberType> { new ContactNumberType(1, "TestContactType") }));
        //    person.Apply(mockEvent);

        //    person.ContactNumbers.Add(new ContactNumber(ContactNumberType.GetById(0), "12345", 44, String.Empty));

        //    //set up search client
        //    var dependencyResolver = new DependencyResolverMock();
        //    var connectionFactory = ConnectionSettingsFactoryHelper.GetHTTPConnection();
        //    var connectionPool = ConnectionSettingsFactoryHelper.GetSingleNodePool();
        //    var settingsProvider = new ConnectionSettingsProvider(connectionFactory, connectionPool);
        //    var connectionManager = new ConnectionManager(settingsProvider);
        //    var clientFactory = new ElasticClientFactory(connectionManager);
        //    var indexManager = new IndexManager(dependencyResolver, clientFactory);
        //    var documentclient = new DocumentController(clientFactory, indexManager);

        //    //to call the server to assert... there should be a test server.!!!!!
        //    var client = clientFactory.GetClient();
        //    //ACT
        //    documentclient.UpsertDocument(person);
        //    //ASSERT
        //    var index = new IndexName { Name = typeof(Person).Name.ToLower(), Type = typeof(Person) };
        //    var getRequest = new GetRequest(index, TypeName.Create<Person>(), person.Id);
        //    var documentResponse = client.Get<Person>(getRequest);
        //    //var p = documentResponse.Source;
        //}
    }
}