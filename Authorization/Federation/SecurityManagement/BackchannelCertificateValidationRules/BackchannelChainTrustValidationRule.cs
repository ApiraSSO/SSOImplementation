using Kernel.Cryptography.Validation;

namespace SecurityManagement.BackchannelCertificateValidationRules
{
    internal class BackchannelChainTrustValidationRule : BackchannelValidationRule
    {
        protected override bool ValidateInternal(BackchannelCertificateValidationContext context)
        {
            context.Validated();
            return true;
        }
    }
}