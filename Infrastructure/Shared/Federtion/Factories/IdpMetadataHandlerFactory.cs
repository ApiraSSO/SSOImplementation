using System;
using System.Collections.Concurrent;
using System.IdentityModel.Metadata;
using System.Linq.Expressions;
using Kernel.Federation.MetaData;

namespace Shared.Federtion.Factories
{
    public partial class IdpMetadataHandlerFactory
    {
        private static ConcurrentDictionary<Type, Func<object, MetadataBase, Uri, Uri>> _cache = new ConcurrentDictionary<Type, Func<object, MetadataBase, Uri, Uri>>();
        public static Func<object, MetadataBase, Uri, Uri> GetDelegateForIdpLocation(Type descriptorType)
        {
            return IdpMetadataHandlerFactory._cache.GetOrAdd(descriptorType, t => IdpMetadataHandlerFactory.BuildDelegate(t));
        }

        private static Func<object, MetadataBase, Uri, Uri> BuildDelegate(Type t)
        {
            if (t == null)
                throw new ArgumentNullException("type");

            var handlerType = typeof(IMetadataHandler<>)
                .MakeGenericType(t);

            var minfo = handlerType.GetMethod("ReadIdpLocation");
            if (minfo == null)
                throw new MissingMethodException("IMetadataHandler", "ReadIdpLocation");

            var handlerPar = Expression.Parameter(typeof(object));
            var bindingPar = Expression.Parameter(typeof(Uri));
            var metadataPar = Expression.Parameter(typeof(MetadataBase));
            var convertHandlerEx = Expression.Convert(handlerPar, handlerType);
            var callEx = Expression.Call(convertHandlerEx, minfo, Expression.Convert(metadataPar,t), bindingPar);
            var lambda = Expression.Lambda<Func<object, MetadataBase, Uri, Uri>>(callEx, handlerPar, metadataPar, bindingPar);
            return lambda.Compile();
        }
    }
}