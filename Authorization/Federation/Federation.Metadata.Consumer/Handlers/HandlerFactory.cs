using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq.Expressions;
using Kernel.Federation.MetaData;

namespace Federation.Metadata.FederationPartner.Handlers
{
    internal class HandlerFactory
    {
        private static ConcurrentDictionary<Type, Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>> _cache = new ConcurrentDictionary<Type, Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>>();
        public static Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>> GetDelegateForIdpDescriptors(Type metadataType, Type descriptorType)
        {
            return HandlerFactory._cache.GetOrAdd(metadataType, t => HandlerFactory.BuildDelegate(t, descriptorType));
        }

        private static Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>> BuildDelegate(Type t, Type descriptorType)
        {
            var handlerType = typeof(IMetadataHandler<>)
                .MakeGenericType(t);
            var minfo = handlerType.GetMethod("GetRoleDescriptors").MakeGenericMethod(descriptorType);
            var handlerPar = Expression.Parameter(typeof(object));
            
            var metadataPar = Expression.Parameter(typeof(MetadataBase));
            var convertHandlerEx = Expression.Convert(handlerPar, handlerType);
            var callEx = Expression.Call(convertHandlerEx, minfo, Expression.Convert(metadataPar,t));
            var lambda = Expression.Lambda<Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>>(callEx, handlerPar, metadataPar);
            return lambda.Compile();
        }
    }
}