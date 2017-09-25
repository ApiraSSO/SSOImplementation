using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kafka.Client.Cfg;
using Kafka.Client.Messages;
using Kafka.Client.Producers;
using Kernel.Serialisation;
using SearchEngine.Infrastructure;

namespace DragonCMS.KafkaClient.ProducerClient
{
    internal class KafkaDispatcherMS : IDocumentDispatcher
    {
        //private readonly ProducerConfigManager _producerConfigManager;
        private readonly ISerializer _serializer;
        //private readonly IResponseHandler _responseHandler;
        public KafkaDispatcherMS(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        //public KafkaDispatcherMS(ProducerConfigManager producerConfigManager)
        //{
        //    this._producerConfigManager = producerConfigManager;
        //}

        public virtual async Task UpsertDocument<TDocument>(IUpsertDocumentContext<TDocument> context) where TDocument : class
        {
            try
            {
                var topicName = context.Document.GetType().Name;
                var brokerConfig = new BrokerConfiguration()
                {
                    BrokerId = 0,
                    Host = "localhost",
                    Port = 9092
                };
                var config = new ProducerConfiguration(new List<BrokerConfiguration> { brokerConfig });
                //var serializer = new BinarySerializer<TDocument>();
                var json = this._serializer.Serialize(context.Document);
                var content = Encoding.Default.GetBytes(json);
                var msg = new Message(content);
                var kafkaProducer = new Producer(config);
                
                var batch = new ProducerData<string, Message>(topicName, msg);
                kafkaProducer.Send(batch);
            }
            catch(Exception e)
            {
                
            }
        }
    }
}