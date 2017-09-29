using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Kernel.Cryptography.Validation
{
    //
    // Summary:
    //     Interface for providing pinned certificate validation, which checks HTTPS communication
    //     against a known good list of certificates to protect against compromised or rogue
    //     CAs issuing certificates for hosts without the knowledge of the host owner.
    public interface ICertificateValidator
    {
        string FederationPartyId { get; }

        X509CertificateValidationMode X509CertificateValidationMode { get; }
        
        void Validate(X509Certificate2 certificate);
        void SetFederationPartyId(string federationPartyId);
    }
}