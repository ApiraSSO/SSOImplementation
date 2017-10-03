using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kernel.Federation.Protocols;

namespace Federation.Protocols.RelayState
{
    internal class RelaystateSerialiser : IRelayStateSerialiser
    {
        private readonly IMessageEncoding _encoding;

        public RelaystateSerialiser(IMessageEncoding encoding)
        {
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

        public void Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object o)
        {
            var encodedTask =  this._encoding.EncodeMessage(o.ToString());
            encodedTask.Wait();
            return encodedTask.Result;
        }

        async Task<object> IRelayStateSerialiser.Deserialize(string data)
        {
            var decoded = await this._encoding.DecodeMessage(data);
            return decoded;
        }
    }
}