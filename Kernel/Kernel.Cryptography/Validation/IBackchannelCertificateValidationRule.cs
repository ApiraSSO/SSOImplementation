using System;
using System.Threading.Tasks;

namespace Kernel.Cryptography.Validation
{
    public interface IBackchannelCertificateValidationRule
    {
        Task Validate(BackchannelCertificateValidationContext context, Func<BackchannelCertificateValidationContext, Task> next);
    }
}