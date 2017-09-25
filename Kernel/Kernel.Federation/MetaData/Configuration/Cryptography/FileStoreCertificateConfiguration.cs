using System;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.CertificateManagement;

namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    public class FileStoreCertificateConfiguration : CertificateStore<FileSystemStore>
    {
        public FileStoreCertificateConfiguration(FileSystemStore store) : base(store)
        {
        }

        public override X509Certificate2 GetX509Certificate2()
        {
            throw new NotImplementedException();
        }
    }
}