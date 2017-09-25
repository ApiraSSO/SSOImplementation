using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Confluent.Kafka.Serialization;

namespace DragonCMS.KafkaClient.Serializers
{
    internal class BinarySerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public T Deserialize(byte[] data)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                return (T)formatter.Deserialize(ms);
            }
        }

        public byte[] Serialize(T data)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, data);
                return ms.ToArray();
            }
        }
    }
}