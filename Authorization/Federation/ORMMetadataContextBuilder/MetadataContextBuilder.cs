﻿using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Data.ORM;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.RelyingParty;
using ORMMetadataContextProvider.Models;
using ORMMetadataContextProvider.RelyingParty;

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

            var relyingParty = this._cacheProvider.Get<TenantContext>(metadataGenerateContext.RelyingPartyId);
            if(relyingParty == null)
            {
                var relyingPartyBuilder = new RelyingPartyContextBuilder(this._dbContext, this._cacheProvider);
                relyingParty = relyingPartyBuilder.BuildRelyingPartyContext(metadataGenerateContext.RelyingPartyId);
            }
            
            return relyingParty.MetadataContext;
        }
        
        internal MetadataContext BuildFromDbSettings(MetadataSettings metadataSettings)
        {
            if (metadataSettings is null)
                throw new ArgumentNullException("metadataSettings");

            var entityDescriptor = metadataSettings.SPDescriptorSettings;
            var entityDescriptorConfiguration = MetadataHelper.BuildEntityDesriptorConfiguration(entityDescriptor);
            var signing = metadataSettings.SigningCredential;

            var signingContext = new MetadataSigningContext(signing.SignatureAlgorithm, signing.DigestAlgorithm);
            signingContext.KeyDescriptors.Add(MetadataHelper.BuildKeyDescriptorConfiguration(signing.Certificates.First(x => x.Use == KeyUsage.Signing && x.IsDefault)));
            var metadataContext = new MetadataContext
            {
                EntityDesriptorConfiguration = entityDescriptorConfiguration,
                MetadataSigningContext = signingContext
            };
            return metadataContext;
        }
        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}