using System;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.CertificateProvider;

namespace ComponentSpace.SAML2.Metadata.Provider.CertificateProviderImplementation
{
    public class CertificateProvider : ICertificateStore
    {
        ICertificateCachePopulator _certificateCachePopulator;

        public CertificateProvider(ICertificateCachePopulator certificateCachePopulator)
        {
            _certificateCachePopulator = certificateCachePopulator;
        }

        public X509Certificate2 GetX509Certificate2()
        {
            X509Certificate2 cert;
            var populator = this._certificateCachePopulator as SertificateCachePopulator;
            if (populator == null)
                throw new InvalidOperationException(String.Format("Expected type:{0} but it was: {1}", typeof(SertificateCachePopulator), this._certificateCachePopulator.GetType()));

            if (!populator.TryGetEntryFromCache(out cert))
                throw new Exception();

            return cert;
        }
    }
}