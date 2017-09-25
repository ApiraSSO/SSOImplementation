using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using Microsoft.Owin.Security.DataProtection;

namespace Microsoft.AspNet.Identity.Owin.Provider.Factories
{
    internal class UserTokenProviderFactory
    {
        private static ConcurrentDictionary<Type, Func<IDataProtector, object>> _cache = new ConcurrentDictionary<Type, Func<IDataProtector, object>>();

        public static Func<IDataProtector, object> GetTokenProviderDelegate(Type t)
        {
            return UserTokenProviderFactory._cache.GetOrAdd(t, key => UserTokenProviderFactory.BuildDelegate(key));
        }

        private static Func<IDataProtector, object> BuildDelegate(Type type)
        {
            var targetType = typeof(DataProtectorTokenProvider<,>)
                .MakeGenericType(type, typeof(string));
            
            var ctr = targetType.GetConstructor(new Type[] { typeof(IDataProtector) });
            var par = Expression.Parameter(typeof(IDataProtector));
            var newExp = Expression.New(ctr, par);
            var lambda = Expression.Lambda<Func<IDataProtector, object>>(newExp, par).Compile();
            return lambda;
        }
    }
}