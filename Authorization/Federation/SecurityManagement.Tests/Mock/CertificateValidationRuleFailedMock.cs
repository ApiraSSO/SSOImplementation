using System;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.Tests.Mock
{
    internal class CertificateValidationRuleFailedMock : ICertificateValidationRule
    {
        public Task Validate(CertificateValidationContext context, Func<CertificateValidationContext, Task> next)
        {
            throw new InvalidOperationException();
        }
    }
}