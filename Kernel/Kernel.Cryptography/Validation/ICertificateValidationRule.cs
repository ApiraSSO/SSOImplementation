using System;
using System.Threading.Tasks;

namespace Kernel.Cryptography.Validation
{
    public interface ICertificateValidationRule
    {
        Task Validate(CertificateValidationContext context, Func<CertificateValidationContext, Task> next);
    }
}