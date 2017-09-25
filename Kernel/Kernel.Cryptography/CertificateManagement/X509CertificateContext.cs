using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.CertificateManagement
{
    public class X509CertificateContext : CertificateContext
    {
        public string StoreName { get; set; }
        public StoreLocation StoreLocation { get; set; }
    }
}