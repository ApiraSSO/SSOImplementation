using System;
using Kernel.Initialisation;
using Nest;

namespace ElasticSearchClient.IndexAPI.PersonIndexMappers
{
    internal abstract class PropertyMapper<T> : IAutoRegisterAsTransient where T : class
    {
        public virtual Func<TypeMappingDescriptor<T>, ITypeMapping> MapProperty(Func<TypeMappingDescriptor<T>, ITypeMapping> next)
        {
            Func<TypeMappingDescriptor<T>, ITypeMapping> func =
                d =>
                {
                    var nextResult = next(d) as TypeMappingDescriptor<T>;
                    if (nextResult == null)
                        nextResult = d;
                    var mapping = this.MapPropertyInternal(nextResult);
                    return mapping;
                };
            return func;
        }

        protected abstract TypeMappingDescriptor<T> MapPropertyInternal(TypeMappingDescriptor<T> descriptor);
    }
}