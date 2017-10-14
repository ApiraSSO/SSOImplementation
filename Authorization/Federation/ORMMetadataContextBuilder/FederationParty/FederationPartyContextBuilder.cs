using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Data.ORM;
using Kernel.Federation.FederationPartner;
using MemoryCacheProvider;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.FederationParty
{
    internal class FederationPartyContextBuilder : IFederationPartyContextBuilder
    {
        private readonly IDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;

        public FederationPartyContextBuilder(IDbContext dbContext, ICacheProvider cacheProvider)
        {
            this._dbContext = dbContext;
            this._cacheProvider = cacheProvider;
        }
        public FederationPartyConfiguration BuildContext(string federationPartyId)
        {
            if (this._cacheProvider.Contains(federationPartyId))
                return this._cacheProvider.Get<FederationPartyConfiguration>(federationPartyId);

            var federationPartyContext = this._dbContext.Set<FederationPartySettings>()
                .FirstOrDefault(x => x.FederationPartyId == federationPartyId);

            var context = new FederationPartyConfiguration(federationPartyId, federationPartyContext.MetadataPath);

            if (federationPartyContext.DefaultNameIdFormat != null)
                context.DefaultNameIdFormat = new Uri(federationPartyContext.DefaultNameIdFormat.Uri);

            context.RefreshInterval = TimeSpan.FromSeconds(federationPartyContext.RefreshInterval);
            context.AutomaticRefreshInterval = TimeSpan.FromDays(federationPartyContext.AutoRefreshInterval);
            this.BuildMetadataContext(context, federationPartyContext.MetadataSettings);
            object policy = new MemoryCacheItemPolicy();
            ((ICacheItemPolicy)policy).SlidingExpiration = TimeSpan.FromDays(1);
            this._cacheProvider.Put(federationPartyId, context,  (ICacheItemPolicy)policy);
            return context;
        }

        private void BuildMetadataContext(FederationPartyConfiguration federationPartyContext, MetadataSettings metadataSettings)
        {
            var metadataContextBuilder = new MetadataContextBuilder(this._dbContext, this._cacheProvider);
            var metadata = metadataContextBuilder.BuildFromDbSettings(metadataSettings);
            federationPartyContext.MetadataContext = metadata;
        }

        public void Dispose()
        {
            if(this._dbContext != null)
                this._dbContext.Dispose();
        }
    }
}