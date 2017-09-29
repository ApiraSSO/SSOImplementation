using System;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.Tests.Mock
{
    internal class CertificateValidationConfigurationProvider : ICertificateValidationConfigurationProvider
    {
        Func<CertificateValidationConfiguration> _func;
        public CertificateValidationConfigurationProvider(Func<CertificateValidationConfiguration> func)
        {
            this._func = func;
        }
        public CertificateValidationConfiguration GetConfiguration(string federationPartyId)
        {
            return this._func();
        }
        public void Dispose()
        {
        }
    }
}
