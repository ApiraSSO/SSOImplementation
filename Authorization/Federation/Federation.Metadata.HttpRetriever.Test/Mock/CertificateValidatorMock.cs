using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Kernel.Cryptography.Validation;

namespace Federation.Metadata.HttpRetriever.Test.Mock
{
    internal class CertificateValidatorMock : ICertificateValidator, IBackchannelCertificateValidator
    {
        public System.ServiceModel.Security.X509CertificateValidationMode X509CertificateValidationMode => throw new NotImplementedException();

        public string FederationPartyId { get; }

        public void SetFederationPartyId(string federationPartyId)
        {
            throw new NotImplementedException();
        }

        public bool Validate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Validate(X509Certificate2 certificate)
        {
            throw new NotImplementedException();
        }
    }
}
