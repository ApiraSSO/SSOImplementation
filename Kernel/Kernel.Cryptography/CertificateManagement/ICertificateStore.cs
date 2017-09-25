using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.CertificateManagement
{
    public interface ICertificateStore
    {
        X509Certificate2 GetX509Certificate2();
        StoreLocation StoreLocation { get; }
    }
}