using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearchClient.IndexAPI.PersonIndexMappers;
using Kernel.DependancyResolver;
using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.IndexAPI
{
    public interface IIndexMapper : IAutoRegisterAsTransient
    {
        CreateIndexDescriptor Map(CreateIndexDescriptor descriptor);
    }
    internal abstract class IndexMapper<T> : IIndexMapper where T : class
    {
        private readonly IDependencyResolver _dependencyResolver;
        protected IEnumerable<PropertyMapper<T>> Mappers;

        public IndexMapper(IDependencyResolver _dependencyResolver)
        {
            this._dependencyResolver = _dependencyResolver;
        }
        
        public virtual CreateIndexDescriptor Map(CreateIndexDescriptor descriptor)
        {
            Func<Func<TypeMappingDescriptor<T>, ITypeMapping>, CreateIndexDescriptor> seed = f =>
            {
                descriptor = descriptor.Mappings(m => m.Map<T>(f));
                return descriptor;
            };

            var res = this.Mappers.Aggregate(seed, (x, next) =>
            {
                var returnRes = new Func<Func<TypeMappingDescriptor<T>, ITypeMapping>, CreateIndexDescriptor>
                (
                    pr =>
                    {
                        x(next.MapProperty(pr));

                        return descriptor;
                    });
                return returnRes;
            });

            var desc = res(new Func<TypeMappingDescriptor<T>, ITypeMapping>(q => null));
            return desc;
        }
    }
}