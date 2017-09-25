using System;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.CertificateValidationRules
{
    internal class EffectiveDateRule : CertificateValidationRule
    {
        protected override void Internal(CertificateValidationContext context)
        {
            var certificate = context.Certificate;
            var effectiveDateString = certificate.GetEffectiveDateString();

            DateTime date;
            DateTime.TryParse(effectiveDateString, out date);
            if (date > DateTime.Now)
                throw new InvalidOperationException("Certificate has expired");
        }
    }
}