using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Data.ORM;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider
{
    public class MetadataContextBuilder : IMetadataContextBuilder
    {
        private readonly IDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;

        public MetadataContextBuilder(IDbContext dbContext, ICacheProvider cacheProvider)
        {
            this._cacheProvider = cacheProvider;
            this._dbContext = dbContext;
        }
        public MetadataContext BuildContext(MetadataGenerateRequest metadataGenerateContext)
        {
            if (metadataGenerateContext == null)
                throw new ArgumentNullException("metadataGenerateContext");

            var metadataSettings = this._dbContext.Set<RelyingPartySettings>()
                .Where(x => x.RelyingPartyId == metadataGenerateContext.RelyingPartyId)
                .Select(r => r.MetadataSettings)
                .FirstOrDefault();
                

            if (metadataSettings is null)
                throw new InvalidOperationException(String.Format("No relyingParty configuration found for relyingPartyId: {0}", metadataGenerateContext.RelyingPartyId));

            var entityDescriptor = metadataSettings.SPDescriptorSettings;
            var entityDescriptorConfiguration = MetadataHelper.BuildEntityDesriptorConfiguration(entityDescriptor);
            var signing = metadataSettings.SigningCredential;
            
            var signingContext = new MetadataSigningContext(signing.SignatureAlgorithm, signing.DigestAlgorithm);
            signingContext.KeyDescriptors.Add(MetadataHelper.BuildKeyDescriptorConfiguration(signing.Certificates.First(x => x.Use == KeyUsage.Signing && x.IsDefault)));
            return new MetadataContext
            {
                EntityDesriptorConfiguration = entityDescriptorConfiguration,
                MetadataSigningContext = signingContext
            };
        }
        
        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}