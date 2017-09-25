using System.Linq;
using Kernel.Cache;
using Kernel.Data.ORM;
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
        public MetadataContext BuildContext()
        {
            var entityDescriptor = this._dbContext.Set<EntityDescriptorSettings>()
                .First();

            var entityDescriptorConfiguration = MetadataHelper.BuildEntityDesriptorConfiguration(entityDescriptor);
            var signing = this._dbContext.Set<SigningCredential>()
                .First();

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