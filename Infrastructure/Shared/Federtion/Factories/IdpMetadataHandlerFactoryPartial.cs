using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq.Expressions;
using Kernel.Federation.MetaData;

namespace Shared.Federtion.Factories
{
    public partial class IdpMetadataHandlerFactory
    {
        private static ConcurrentDictionary<Type, Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>> _cache1 = new ConcurrentDictionary<Type, Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>>();
        public static Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>> GetDelegateForIdpDescriptors(Type metadataType, Type descriptorType)
        {
            return IdpMetadataHandlerFactory._cache1.GetOrAdd(metadataType, t => IdpMetadataHandlerFactory.BuildDelegate(t, descriptorType));
        }

        private static Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>> BuildDelegate(Type t, Type descriptorType)
        {
            if (t == null)
                throw new ArgumentNullException("metadata type");

            if (descriptorType == null)
                throw new ArgumentNullException("descriptor type");

            var handlerType = typeof(IMetadataHandler<>)
                .MakeGenericType(t);

            var minfo = handlerType.GetMethod("GetRoleDescriptors");
            if (minfo == null)
                throw new MissingMethodException("IMetadataHandler", "GetRoleDescriptors");

            var minfoGeneric = minfo.MakeGenericMethod(descriptorType);
            var handlerPar = Expression.Parameter(typeof(object));
            
            var metadataPar = Expression.Parameter(typeof(MetadataBase));
            var convertHandlerEx = Expression.Convert(handlerPar, handlerType);
            var callEx = Expression.Call(convertHandlerEx, minfoGeneric, Expression.Convert(metadataPar,t));
            var lambda = Expression.Lambda<Func<object, MetadataBase, IEnumerable<SingleSignOnDescriptor>>>(callEx, handlerPar, metadataPar);
            return lambda.Compile();
        }
    }
}