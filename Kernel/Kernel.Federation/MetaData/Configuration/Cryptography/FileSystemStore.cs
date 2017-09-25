using System.Security;

namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    public class FileSystemStore
    {
        public string SertificateFilePath { get; set; }
        public SecureString CertificatePassword { get; set; }
    }
}