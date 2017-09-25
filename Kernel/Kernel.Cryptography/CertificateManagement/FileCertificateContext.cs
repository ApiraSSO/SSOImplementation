using System.Security;

namespace Kernel.Cryptography.CertificateManagement
{
    public class FileCertificateContext : CertificateContext
    {
        public string CertificatePath { get; set; }
        public SecureString Password { get; set; }
    }
}