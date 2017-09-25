using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;

namespace Provider.EntityFramework.CustomServices
{
    internal class DbModelCacheKeyFactory
    {
        /// <summary>
        ///     Creates the cache key for given context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>IDbModelCacheKey.</returns>
        public IDbModelCacheKey Create(DbContext context)
        {
            //A string value provided by IDbModelCacheKeyProvider interface. Implement this interface in contex to generate the key. Default is Null.
            string customKey = null;

            var cacheKeyProvider = context as IDbModelCacheKeyProvider;

            if (cacheKeyProvider != null)
                customKey = cacheKeyProvider.CacheKey;

            var providerFactory = DbProviderServices.GetProviderFactory(context.Database.Connection);

            var providerName = DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>(providerFactory).Name;

            return new DbModelCacheKey(context.GetType(), providerName, providerFactory.GetType(), customKey);
        }
    }
}