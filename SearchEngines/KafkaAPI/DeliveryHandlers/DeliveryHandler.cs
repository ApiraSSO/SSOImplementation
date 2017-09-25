using Confluent.Kafka;

namespace KafkaClient.DeliveryHandlers
{
    internal class DeliveryHandler<TKey, TValue> : IDeliveryHandler<TKey, TValue>
    {
        public bool MarshalData
        {
            get
            {
                return true;
            }
        }

        public void HandleDeliveryReport(Message<TKey, TValue> message)
        {
            
        }
    }
}