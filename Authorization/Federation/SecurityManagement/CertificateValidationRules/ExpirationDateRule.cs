using System;
using Kernel.Cryptography.Validation;

namespace SecurityManagement.CertificateValidationRules
{
    internal class ExpirationDateRule : CertificateValidationRule
    {
        protected override void Internal(CertificateValidationContext context)
        {
            var certificate = context.Certificate;
            var expirationDateString = certificate.GetExpirationDateString();
            
            DateTime date;
            DateTime.TryParse(expirationDateString, out date);
            if (date < DateTime.Now)
                throw new InvalidOperationException("Certificate has expired");
        }
    }
}