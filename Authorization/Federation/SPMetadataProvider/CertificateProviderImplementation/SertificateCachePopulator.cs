using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cache;
using Kernel.Federation.CertificateProvider;
using Kernel.Federation.FederationConfiguration;
using MemoryCacheProvider.Dependencies;

namespace WsFederationMetadataProvider.CertificateProviderImplementation
{
    public class SertificateCachePopulator : FileDependencyController, ICertificateCachePopulator
    {
        IConfiguration _configuration;

        ICacheProvider _cache;

        public SertificateCachePopulator(ICacheProvider cache, IConfiguration configuration)
        {
            _cache = cache;

            _configuration = configuration;
        }

        protected override IList<string> FilePaths
        {
            get { return new List<string> { _configuration.SertificatePath }; }
        }

        public string CacheKey
        {
            get { return "saml2Cert"; }
        }

        public X509Certificate2 PopulateCache()
        {
            var policy = RegisterDependency(_configuration.RegisterFileDepenencyMonitor);

            var cert = new X509Certificate2(_configuration.SertificatePath, _configuration.SertificatePassword, X509KeyStorageFlags.MachineKeySet);

            _cache.Post(CacheKey, cert, policy);

            return cert;
        }

        public bool IsStale()
        {
            throw new NotImplementedException();
        }

       

        public bool TryGetEntryFromCache(out X509Certificate2 entry)
        {
            entry = _cache.Get<X509Certificate2>(CacheKey);

            if (entry == null)
                entry = PopulateCache();

            return entry != null;
        }
        
        public X509Certificate2 Refresh(ICacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public void Populate()
        {
            this.PopulateCache();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool v)
        {
        }
    }
}