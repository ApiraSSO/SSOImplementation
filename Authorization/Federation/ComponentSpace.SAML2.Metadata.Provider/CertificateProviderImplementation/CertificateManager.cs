using System.Security;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.CertificateManagement;

namespace ComponentSpace.SAML2.Metadata.Provider.CertificateProviderImplementation
{
    public class CertificateManager : ICertificateManager
    {
        public X509Certificate2 GetCertificate(string path, SecureString password)
        {
            return new X509Certificate2(path, password);
        }
    }
}