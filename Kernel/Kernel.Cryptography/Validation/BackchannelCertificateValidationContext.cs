using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.Validation
{
    public class BackchannelCertificateValidationContext : CertificateValidationContext
    {
        public X509Chain Chain { get; }
        public SslPolicyErrors SslPolicyErrors { get; }
        public BackchannelCertificateValidationContext(X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            : base(certificate)
        {
            this.Chain = chain;
            this.SslPolicyErrors = sslPolicyErrors;
        }
    }
}