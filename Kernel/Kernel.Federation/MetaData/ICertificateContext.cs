using System.Security;

namespace Kernel.Federation.MetaData
{
    public interface ICertificateContext
    {
        string Usage { get; set; }
        string SertificateFilePath { get; set; }
        SecureString CertificatePassword { get; set; }
        bool DefaultForMetadataSigning { get; set; }
    }
}