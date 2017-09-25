using System.Security;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProvider.Metadata
{
    public class CertificateContext : ICertificateContext
    {
        public string Usage { get; set; }

        public string SertificateFilePath { get; set; }

        public SecureString CertificatePassword { get; set; }

        public bool DefaultForMetadataSigning { get; set; }
    }
}