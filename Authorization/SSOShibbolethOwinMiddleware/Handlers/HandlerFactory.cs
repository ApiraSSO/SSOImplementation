﻿using System;
using System.Collections.Concurrent;
using System.IdentityModel.Metadata;
using System.Linq.Expressions;
using Kernel.Federation.MetaData;

namespace SSOOwinMiddleware.Handlers
{
    internal class HandlerFactory
    {
        private static ConcurrentDictionary<Type, Func<object, MetadataBase, Uri, Uri>> _cache = new ConcurrentDictionary<Type, Func<object, MetadataBase, Uri, Uri>>();
        public static Func<object, MetadataBase, Uri, Uri> GetDelegateForIdpLocation(Type descriptorType)
        {
            return HandlerFactory._cache.GetOrAdd(descriptorType, t => HandlerFactory.BuildDelegate(t));
        }

        private static Func<object, MetadataBase, Uri, Uri> BuildDelegate(Type t)
        {
            var handlerType = typeof(IMetadataHandler<>)
                .MakeGenericType(t);
            var minfo = handlerType.GetMethod("ReadIdpLocation");
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