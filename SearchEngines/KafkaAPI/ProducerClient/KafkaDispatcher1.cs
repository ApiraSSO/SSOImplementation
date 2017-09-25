using System;
using System.Threading.Tasks;
using DragonCMS.Common.SearchEngine;
using DragonCMS.KafkaClient.Configs.ConfigurationPrviders;
using DragonCMS.KafkaClient.Configs.ProducerConfiguration;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;

namespace DragonCMS.KafkaClient.ProducerClient
{
    internal class KafkaDispatcher1 : IDocumentDispatcher
    {
        private readonly ProducerConfigManager _producerConfigManager;
        //private readonly IIndexManager _indexManager;
        //private readonly IResponseHandler _responseHandler;

        public KafkaDispatcher1(ProducerConfigManager producerConfigManager)
        {
            this._producerConfigManager = producerConfigManager;
        }

        public virtual async Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            var topicName = context.Document.GetType().Name;
            var config = this._producerConfigManager.GetConfiguration(x => (x.ConfigurationScope & ConfigurationScope.Producer) == ConfigurationScope.Producer);
            var options = new KafkaOptions
            (new Uri("http://localhost:9092"));
            var router = new BrokerRouter(options);

            var client = new KafkaNet.Producer(router);
            client.SendMessageAsync(topicName,
                new[]{ new Message("!!!Test message sent from alternative .Net => kafka provider!!!") }, 
                -1, 
                TimeSpan.FromSeconds(5))
               .Wait();

            //var valueSerialiser = new BinarySerializer<TDocument>();
            // var keySerialiser = new BinarySerializer<Guid>();
            //var deliveryHandler = new DeliveryHandler<Guid, TDocument>();
            //using (var producer = new Producer<Guid, TDocument>(config, keySerialiser, valueSerialiser))
            //{
            //    var deliveryReport = await producer.ProduceAsync(topicName, context.Id, context.Document);

            //    //producer.ProduceAsync(topicName, null, context.Document, deliveryHandler);
            //    //producer.Flush();
            //}
        }
    }
}