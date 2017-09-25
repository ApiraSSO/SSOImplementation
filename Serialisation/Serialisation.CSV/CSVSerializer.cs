using System;
using System.Collections.Generic;
using System.IO;
using Kernel.Serialisation;

namespace Serialisation.CSV
{
    public class CSVSerializer : ISerializer
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
