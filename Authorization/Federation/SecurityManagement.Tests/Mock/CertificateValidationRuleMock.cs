using System;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.Tests.Mock
{
    internal class CertificateValidationRuleMock : ICertificateValidationRule
    {
        public Task Validate(CertificateValidationContext context, Func<CertificateValidationContext, Task> next)
        {
            return next(context);
        }
    }
}