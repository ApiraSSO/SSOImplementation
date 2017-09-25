using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using DragonCMS.KafkaClient.Serializers;
using KafkaClient.Configs.ConfigurationPrviders;
using KafkaClient.Configs.ProducerConfiguration;
using KafkaClient.DeliveryHandlers;
using SearchEngine.Infrastructure;

namespace KafkaClient.ProducerClient
{
    internal class KafkaDispatcherConfuence : IDocumentDispatcher
    {
        private readonly ProducerConfigManager _producerConfigManager;
        //private readonly IIndexManager _indexManager;
        //private readonly IResponseHandler _responseHandler;

        public KafkaDispatcherConfuence(ProducerConfigManager producerConfigManager)
        {
            this._producerConfigManager = producerConfigManager;
        }

        public virtual async Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            var topicName = context.Document.GetType().Name;
            var config = this._producerConfigManager.GetConfiguration(x => (x.ConfigurationScope & ConfigurationScope.Producer) == ConfigurationScope.Producer);
            var valueSerialiser = new BinarySerializer<TDocument>();
            var keySerialiser = new BinarySerializer<Guid>();
            var deliveryHandler = new DeliveryHandler<Guid, TDocument>();
            using (var producer = new Producer<Guid, TDocument>(config, keySerialiser, valueSerialiser))
            {
                var deliveryReport = await producer.ProduceAsync(topicName, context.Id, context.Document);

                //producer.ProduceAsync(topicName, null, context.Document, deliveryHandler);
                //producer.Flush();
            }
        }
    }
}