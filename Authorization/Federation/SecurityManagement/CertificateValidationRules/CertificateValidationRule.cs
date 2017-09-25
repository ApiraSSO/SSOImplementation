using System;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.CertificateValidationRules
{
    internal abstract class CertificateValidationRule : ICertificateValidationRule
    {
        public Task Validate(CertificateValidationContext context, Func<CertificateValidationContext, Task> next)
        {
            this.Internal(context);
            return next(context);
        }

        protected abstract void Internal(CertificateValidationContext context);
    }
}