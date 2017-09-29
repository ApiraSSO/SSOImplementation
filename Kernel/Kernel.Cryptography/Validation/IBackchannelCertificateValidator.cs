using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Kernel.Cryptography.Validation
{
    //
    // Summary:
    //     Interface for providing pinned certificate validation, which checks HTTPS communication
    //     against a known good list of certificates to protect against compromised or rogue
    //     CAs issuing certificates for hosts without the knowledge of the host owner.
    public interface IBackchannelCertificateValidator
    {
        //
        // Summary:
        //     Verifies the remote Secure Sockets Layer (SSL) certificate used for authentication.
        //
        // Parameters:
        //   sender:
        //     An object that contains state information for this validation.
        //
        //   certificate:
        //     The certificate used to authenticate the remote party.
        //
        //   chain:
        //     The chain of certificate authorities associated with the remote certificate.
        //
        //   sslPolicyErrors:
        //     One or more errors associated with the remote certificate.
        //
        // Returns:
        //     A Boolean value that determines whether the specified certificate is accepted
        //     for authentication.
        bool Validate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors);
    }
}