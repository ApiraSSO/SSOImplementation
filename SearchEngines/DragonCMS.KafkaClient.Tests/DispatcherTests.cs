using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.Dependencies;
using DragonCMS.Common.SearchEngine;
using DragonCMS.KafkaClient.Configs.ConfigurationPrviders;
using DragonCMS.KafkaClient.Configs.ConfigurationPrviders.ConsumerConfigurationProviders;
using DragonCMS.KafkaClient.Configs.ConfigurationPrviders.ProducerConfigurationProviders;
using DragonCMS.KafkaClient.Configs.ProducerConfiguration;
using DragonCMS.KafkaClient.ProducerClient;
using DragonCMS.KafkaClient.Serializers;
using DragonCMS.KafkaClient.Tests.Factories;
using DragonCMS.KafkaClient.Tests.MockData;
using NUnit.Framework;

namespace DragonCMS.KafkaClient.Tests
{
    [TestFixture]
    //[Ignore("Infrastrcucture tests. To be ignored. No need to run.")]
    internal class DispatcherTests
    {
        [Test]
        public void DispatchToKafkaServerTest()
        {
            //ARRANGE
            var documentId = Guid.NewGuid();
            var document = new ParentTestClass { Email = "Email@domain.com", IntField = 10, DateFiled = DateTimeOffset.Now, Child = new ChildClass { Name = "Child1", ChildEmail = "ChildEmail@domain.com", ChildIntField = 100, ChildDateFiled = DateTimeOffset.Now } };
            document.Child.Parent = document;

            var dependencyResolver = new DependencyResolverMock();

            //var client = SearchClientFactory.GetClient();
            //var clientFactory = SearchClientFactory.GetClientFactory();
            //SearchClientFactory.RegisterDependencies(dependencyResolver);

            ////create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            var producerConfigManager = new ProducerConfigManager(dependencyResolver);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ClientIdProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new EndPoindProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ProducerTopicConfigProvider(), Lifetime.Transient);
            ////set up document CRUD controller and create a mock document
            //var responseHandler = new ResponseHandler();
            //var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            //var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            //var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                var context = new UpsertDocumentContext<ParentTestClass>(documentId) { Document = document, IndexContext = indexContext };
                var dispatcher = new KafkaDispatcherConfuence(producerConfigManager);
                //ACT
                dispatcher.UpsertDocument(context);
                    //.Wait();
                
                //ASSERT
            }
            finally
            {

            }
        }

        [Test]
        public void DispatchToKafkaServerTest1()
        {
            //ARRANGE
            var documentId = Guid.NewGuid();
            var document = PersonAggregateFactory.BuildPersonSearchModel(documentId, "Daniel1", "Georgiev1");

            var dependencyResolver = new DependencyResolverMock();
            
            ////create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(EsPersonSearch).Name, indexId);
            var indexContext = new IndexContext(typeof(EsPersonSearch), indexName);
            var producerConfigManager = new ProducerConfigManager(dependencyResolver);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ClientIdProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new EndPoindProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ProducerTopicConfigProvider(), Lifetime.Transient);
            
            
            try
            {
                var context = new UpsertDocumentContext<EsPersonSearch>(documentId) { Document = document, IndexContext = indexContext };
                var dispatcher = new KafkaDispatcherConfuence(producerConfigManager);
                var documentController = new DocumentControllerMock(dispatcher);
                //ACT
                documentController.UpsertDocument(context);
                //.Wait();

                //ASSERT
            }
            finally
            {

            }
        }

        [Test]
        public void RecieveMessageFromKafkaTest()
        {
            //ARRANGE
            var recieved = false;
            var documentId = Guid.NewGuid();
            var document = new ParentTestClass();

            var dependencyResolver = new DependencyResolverMock();

            //var client = SearchClientFactory.GetClient();
            //var clientFactory = SearchClientFactory.GetClientFactory();
            //SearchClientFactory.RegisterDependencies(dependencyResolver);

            ////create an unique index
            var indexId = Guid.NewGuid();
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId);
            var indexContext = new IndexContext(typeof(ParentTestClass), indexName);
            var producerConfigManager = new ProducerConfigManager(dependencyResolver);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ClientIdProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new EndPoindProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new ProducerTopicConfigProvider(), Lifetime.Transient);
            dependencyResolver.RegisterFactory<IConfigurationProvider>((t) => new GroupIdProvider(), Lifetime.Transient);
            ////set up document CRUD controller and create a mock document
            //var responseHandler = new ResponseHandler();
            //var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            //var documentDispatcher = new DocumentDispatcher(clientFactory, indexManager, responseHandler);
            //var documentclient = new DocumentController(documentDispatcher, indexManager, responseHandler);
            try
            {
                
                //var context = new UpsertDocumentContext<ParentTestClass>(documentId) { Document = document, IndexContext = indexContext };
                //var dispatcher = new KafkaDispatcher(producerConfigManager);
                //ACT
                var topic = typeof(ParentTestClass).Name;
                var cancelled = false;
                //var time = new Timer(new TimerCallback((_) => cancelled = true), null, 5000, 5000);
                var valueSerializer = new BinarySerializer<ParentTestClass>();
                var keySerializer = new BinarySerializer<Guid>();
                var config = producerConfigManager.GetConfiguration(x => (x.ConfigurationScope & ConfigurationScope.Consumer) == ConfigurationScope.Consumer);
                using (var consumer = new Consumer<Guid, ParentTestClass>(config, keySerializer, valueSerializer))
                {
                    var metaData = consumer.GetMetadata(true);
                    
                    consumer.OnMessage += (_, msg)
                        =>
                    {
                        consumer.CommitAsync(msg).Wait();
                        recieved = true;
                        cancelled = true;
                    };

                    consumer.OnPartitionEOF += (_, end)
                        => Console.WriteLine($"Reached end of topic {end.Topic} partition {end.Partition}.");

                    consumer.OnError += (_, error) =>
                    {
                        Console.WriteLine($"Error: {error}");
                        cancelled = true;
                    };

                    consumer.Subscribe(topic);
                    var tp = new TopicPartition(topic, 0);
                    consumer.CommitAsync(new[] { new TopicPartitionOffset(tp, new Offset(15))});
                    var possition = consumer.Position(new[] { tp });
                    var committed = consumer.Committed(new[] { new TopicPartition(topic, 0) }, TimeSpan.FromMilliseconds(5000));
                    while (!cancelled)
                    {
                        consumer.Poll(TimeSpan.FromMilliseconds(100));
                    }
                    Assert.True(recieved);
                }
            }
            finally
            {

            }
        }
    }
}