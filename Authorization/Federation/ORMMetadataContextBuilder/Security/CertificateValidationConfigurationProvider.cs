using System;
using System.Linq;
using Kernel.Cache;
using Kernel.Cryptography.Validation;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

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
            var relyingPartyId = RelyingPartyIdentifierHelper.GetRelyingPartyIdFromRequestOrDefault();
            var settings = this._dbContext.Set<RelyingPartySettings>()
                .Where(x => x.RelyingPartyId == relyingPartyId)
                .Select(r => r.SecuritySettings)
                .FirstOrDefault();

            if (settings is null)
                throw new InvalidOperationException(String.Format("No relyingParty configuration found for relyingPartyId: {0}", relyingPartyId));

            var configuration = new CertificateValidationConfiguration
            {
                X509CertificateValidationMode = settings.X509CertificateValidationMode,
                UsePinningValidation = settings.PinnedValidation,
                BackchannelPinningValidator = new Kernel.Data.TypeDescriptor(settings.PinnedTypeValidator)
            };
            var rules = settings.CertificateValidationRules.Where(x => x.Type != null)
                .ToList();
            rules.Aggregate(configuration.ValidationRules, (t, next) =>
            {
                t.Add(new ValidationRuleDescriptor(next.Type));
                return t;
            });
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