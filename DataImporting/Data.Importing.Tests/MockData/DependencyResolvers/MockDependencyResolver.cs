using System;
using System.Collections.Generic;
using Kernel.DependancyResolver;

namespace Data.Importing.Tests.MockData.DependencyResolvers
{
    class MockDependencyResolver : IDependencyResolver
    {
        public bool Contains<T>()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Type type)
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

        public IDependencyResolver RegisterFactory<T>(Func<T> factory, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterFactory<T>(Func<Type, T> factory, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterFactory(Type type, Func<Type, object> factory, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance(Type t, string name, object instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance<T>(string name, T instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterInstance<T>(T instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterNamedType(Type type, string name, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterType<T>(Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IDependencyResolver RegisterType(Type type, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve<T>(out T resolved)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, out object resolved)
        {
            throw new NotImplementedException();
        }
    }
}