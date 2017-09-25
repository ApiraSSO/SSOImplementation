using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.MetaData.Configuration.Cryptography;

namespace SecurityManagement
{
    public class CertificateManager : ICertificateManager
    {
        public X509Certificate2 GetCertificate(string path, SecureString password)
        {
            return new X509Certificate2(path, password);
        }

        public X509Certificate2 GetCertificate(ICertificateStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");
            return store.GetX509Certificate2();
        }

        public X509Certificate2 GetCertificateFromContext(CertificateContext certContext)
        {
            var store = this.GetStoreFromContext(certContext);
            return this.GetCertificate(store);
        }

        public ICertificateStore GetStoreFromContext(CertificateContext certContext)
        {
            if (certContext is X509CertificateContext)
                return new X509StoreCertificateConfiguration(certContext);

            throw new NotSupportedException(String.Format("Certificate context of type: {0} is not supported.", certContext.GetType().Name));
        }
    }
}