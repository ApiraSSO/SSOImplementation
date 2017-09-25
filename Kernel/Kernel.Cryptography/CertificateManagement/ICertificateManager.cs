using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.CertificateManagement
{
    public interface ICertificateManager
    {
        X509Certificate2 GetCertificate(string path, SecureString password);
        X509Certificate2 GetCertificate(ICertificateStore store);

        ICertificateStore GetStoreFromContext(CertificateContext certContext);
        X509Certificate2 GetCertificateFromContext(CertificateContext certContext);
    }
}
