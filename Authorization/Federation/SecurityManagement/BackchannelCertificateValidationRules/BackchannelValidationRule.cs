using System;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.BackchannelCertificateValidationRules
{
    internal abstract class BackchannelValidationRule : IBackchannelCertificateValidationRule
    {
        public async Task Validate(BackchannelCertificateValidationContext context, Func<BackchannelCertificateValidationContext, Task> next)
        {
            var validationResult = this.ValidateInternal(context);
            if (!validationResult)
                return;

            await next(context);
        }

        protected abstract bool ValidateInternal(BackchannelCertificateValidationContext context);
    }
}