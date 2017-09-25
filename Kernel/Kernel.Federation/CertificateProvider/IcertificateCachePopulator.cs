using System.Security.Cryptography.X509Certificates;
using Kernel.Cache;

namespace Kernel.Federation.CertificateProvider
{
    public interface ICertificateCachePopulator : ICachePopulator<X509Certificate2>
    {
    }
}