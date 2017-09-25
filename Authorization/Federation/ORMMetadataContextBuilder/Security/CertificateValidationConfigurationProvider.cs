using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Cryptography.Validation;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models.GlobalConfiguration;

namespace ORMMetadataContextProvider.Security
{
    internal class CertificateValidationConfigurationProvider : ICertificateValidationConfigurationProvider
    {
        private readonly IDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;

        public CertificateValidationConfigurationProvider(IDbContext dbContext, ICacheProvider cacheProvider)
        {
            this._dbContext = dbContext;
            this._cacheProvider = cacheProvider;
        }
        
        public CertificateValidationConfiguration GetConfiguration()
        {
            var settings = this._dbContext.Set<SecuritySettings>()
                .First();
            var configuration = new CertificateValidationConfiguration
            {
                X509CertificateValidationMode = settings.X509CertificateValidationMode,
                UsePinningValidation = settings.PinnedValidation
            };
            return configuration;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (this._dbContext != null)
                this._dbContext.Dispose();
        }
    }
}