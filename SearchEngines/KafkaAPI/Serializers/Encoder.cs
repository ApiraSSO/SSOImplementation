using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafka.Client.Messages;
using Kafka.Client.Serialization;

namespace DragonCMS.KafkaClient.Serializers
{
    class CMSEncoder<T> : IEncoder<T>
    {
        public Message ToMessage(T data)
        {
            throw new NotImplementedException();
        }
    }
}
