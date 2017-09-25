using Kernel.Cryptography.Validation;

namespace SecurityManagement.BackchannelCertificateValidationRules
{
    internal class BackchannelNoValidationRule : BackchannelValidationRule
    {
        protected override bool ValidateInternal(BackchannelCertificateValidationContext context)
        {
            context.Validated();
            return true;
        }
    }
}