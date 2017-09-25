using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Data.ORM;
using Kernel.Federation.RelyingParty;
using MemoryCacheProvider;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.RelyingParty
{
    internal class RelyingPartyContextBuilder : IRelyingPartyContextBuilder
    {
        private readonly IDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;

        public RelyingPartyContextBuilder(IDbContext dbContext, ICacheProvider cacheProvider)
        {
            this._dbContext = dbContext;
            this._cacheProvider = cacheProvider;
        }
        public RelyingPartyContext BuildRelyingPartyContext(string relyingPartyId)
        {
            if (this._cacheProvider.Contains(relyingPartyId))
                return this._cacheProvider.Get<RelyingPartyContext>(relyingPartyId);

            var relyingPartyContext = this._dbContext.Set<RelyingPartySettings>()
                .FirstOrDefault(x => x.RelyingPartyId == relyingPartyId);

            var context = new RelyingPartyContext(relyingPartyId, relyingPartyContext.MetadataPath);
            context.RefreshInterval = TimeSpan.FromSeconds(relyingPartyContext.RefreshInterval);
            context.AutomaticRefreshInterval = TimeSpan.FromDays(relyingPartyContext.AutoRefreshInterval);
            object policy = new MemoryCacheItemPolicy();
            ((ICacheItemPolicy)policy).SlidingExpiration = TimeSpan.FromDays(1);
            this._cacheProvider.Put(relyingPartyId, context,  (ICacheItemPolicy)policy);
            return context;
        }

        public void Dispose()
        {
            if(this._dbContext != null)
                this._dbContext.Dispose();
        }
    }
}