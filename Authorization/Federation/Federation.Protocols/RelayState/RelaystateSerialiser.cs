using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;
using Kernel.Serialisation;
using Serialisation.JSON;

namespace Federation.Protocols.RelayState
{
    internal class RelaystateSerialiser : IRelayStateSerialiser
    {
        private readonly IMessageEncoding _encoding;
        private readonly IJsonSerialiser _jsonSerialiser;
        public RelaystateSerialiser(IJsonSerialiser jsonSerialiser, IMessageEncoding encoding)
        {
            this._jsonSerialiser = jsonSerialiser;
            this._encoding = encoding;
        }
        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            throw new NotImplementedException();
        }
        
        public object Deserialize(string data)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Serialize(object data)
        {
            var jsonString = this._jsonSerialiser.Serialize(data);
            var encoded = await this._encoding.EncodeMessage(jsonString);
            return encoded;
        }

        public void Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }
        
        async Task<object> IRelayStateSerialiser.Deserialize(string data)
        {
            var decoded = await this._encoding.DecodeMessage(data);
            var deserialised = this._jsonSerialiser.Deserialize(decoded);
            return deserialised;
        }

        string ISerializer.Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}