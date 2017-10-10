using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serialisation.JSON;

namespace Federation.Protocols.Test.Mock
{
    internal class JsonSerialiserMock : IJsonSerialiser
    {
        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(string data)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream stream, object[] o)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object o)
        {
            throw new NotImplementedException();
        }
    }
}
