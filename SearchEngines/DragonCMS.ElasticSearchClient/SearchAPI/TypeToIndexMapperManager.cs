using System;
using System.Collections.Generic;
using Kernel.Initialisation;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI
{
    internal class TypeToIndexMapperManager : ITypeToIndexMapperManager, IAutoRegisterAsTransient
    {
        private static IDictionary<Type, ITypeToIndexMapper> typeMappers = new Dictionary<Type, ITypeToIndexMapper>();
        private static bool initialised;
        public TypeToIndexMapperManager()

        {
            if (initialised)
                return;

            var resolver = ApplicationConfiguration.Instance.DependencyResolver;
            var mappers = resolver.ResolveAll<ITypeToIndexMapper>();
            foreach(var m in mappers)
            {
                var targetType = m.GetType();
                TypeToIndexMapperManager.typeMappers[targetType] = m;
                m.RegisterMapper(this);
            }
            TypeToIndexMapperManager.initialised = true;
        }
       
        public ITypeToIndexMapper GetMapper(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            
            
            if (!TypeToIndexMapperManager.typeMappers.Keys.Contains(type))
                throw new InvalidOperationException(String.Format("No mapper for type found: {0}", type.Name));
            return TypeToIndexMapperManager.typeMappers[type];
        }

        public ITypeToIndexMapperManager RegisterMapper(Type type, ITypeToIndexMapper mapper)
        {
            TypeToIndexMapperManager.typeMappers[type] = mapper;
            return this;
        }
    }
}