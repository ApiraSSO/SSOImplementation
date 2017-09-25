using System;
using System.Collections.Generic;
using Kernel.Cache;

namespace ORMMetadataContextProvider.Tests.Mock
{
    internal class CacheProviderMock : ICacheProvider
    {
        public event EventHandler WrittenTo;
        public event EventHandler ReadFrom;

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public object Delete(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Initialise()
        {
            throw new NotImplementedException();
        }

        public void Post(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Post(string key, object value, ICacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, object value, ICacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, T> TypeOf<T>()
        {
            throw new NotImplementedException();
        }
    }
}
