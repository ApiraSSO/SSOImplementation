using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cache;
using Kernel.Federation.CertificateProvider;
using Kernel.Federation.FederationConfiguration;
using MemoryCacheProvider.Dependencies;

namespace ComponentSpace.SAML2.Metadata.Provider.CertificateProviderImplementation
{
    public class SertificateCachePopulator : FileDependencyController, ICertificateCachePopulator
    {
        IConfiguration _iSAMLConfiguration;

        ICacheProvider _cache;

        public SertificateCachePopulator(ICacheProvider cache, IConfiguration iSAMLConfiguration)
        {
            _cache = cache;

            _iSAMLConfiguration = iSAMLConfiguration;
        }

        public string CacheKey
        {
            get { return "saml2Cert"; }
        }

        public X509Certificate2 PopulateCache()
        {
            var policy = RegisterDependency(_iSAMLConfiguration.RegisterFileDepenencyMonitor);

            var cert = new X509Certificate2(_iSAMLConfiguration.SertificatePath, _iSAMLConfiguration.SertificatePassword, X509KeyStorageFlags.MachineKeySet);

            _cache.Post(CacheKey, cert, policy);

            return cert;
        }

        public bool IsStale()
        {
            throw new NotImplementedException();
        }

        public void Populate()
        {
            this.PopulateCache();
        }

        public X509Certificate2 Refresh(ICacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public bool TryGetEntryFromCache(out X509Certificate2 entry)
        {
            entry = _cache.Get<X509Certificate2>(this.CacheKey);

            if (entry == null)
                entry = this.PopulateCache();

            return entry != null;
        }
        
        protected override IList<string> FilePaths
        {
            get { return new List<string> { _iSAMLConfiguration.SertificatePath }; }
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