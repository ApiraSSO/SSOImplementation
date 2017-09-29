using Kernel.Cryptography.Validation;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class CertificateValidationConfigurationProvider : ICertificateValidationConfigurationProvider
    {
        public CertificateValidationConfiguration GetConfiguration(string federationPartyId)
        {
            return new CertificateValidationConfiguration
            {
                UsePinningValidation = false,
                X509CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom
            };
        }
        public void Dispose()
        {
        }
    }
}