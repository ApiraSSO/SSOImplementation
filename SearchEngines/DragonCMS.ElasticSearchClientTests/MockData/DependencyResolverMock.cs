using System;
using System.Collections.Generic;
using System.Linq;
using DragonCMS.Common.Dependencies;

namespace DragonCMS.ElasticSearchClientTests.MockData
{
    internal class DependencyResolverMock : IDependencyResolver
    {
        private IDictionary<Type, Func<Type, object>> _cache = new Dictionary<Type, Func<Type, object>>();
        private IList<Tuple<Type, Func<Type, object>>> _cache1 = new List<Tuple<Type, Func<Type, object>>>();
        public bool Contains(Type type)
        {
            throw new NotImplementedException();
        }

        public bool Contains<T>()
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterFactory(Type type, Func<Type, object> factory, Lifetime lifetime)
        {
            if (this._cache.ContainsKey(type))
                this._cache1.Add(new Tuple<Type, Func<Type, object>>(type, factory));
            else
                this._cache[type] = factory;
            return this;
        }

        public IDependencyResolver RegisterFactory<T>(Func<Type, T> factory, Lifetime lifetime)
        {
            return this.RegisterFactory(typeof(T), new Func<Type, object>(t => (object)factory(t)), lifetime);
        }

        public IDependencyResolver RegisterFactory<T>(Func<T> factory, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance(Type t, string name, object instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance<T>(T instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance<T>(string name, T instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterNamedType(Type type, string name, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterType(Type type, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterType<T>(Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            if (!this._cache.ContainsKey(type))
                return null;
            return this._cache[type](type);
        }

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            var all = this._cache.Where(c => c.Key == type)
                .Select(p => p.Value(type))
                .Union(this._cache1.Where(c => c.Item1 == type).Select(p => p.Item2(type)));

            return all;
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return this.ResolveAll(typeof(T)).Cast<T>();
        }

        public bool TryResolve(Type type, out object resolved)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve<T>(out T resolved)
        {
            throw new NotImplementedException();
        }
    }
}