using System.Collections.Generic;

namespace Kernel.Cryptography.Validation
{
    public interface ICertificateValidatorResolver
    {
        IEnumerable<ICertificateValidator> Resolve();
    }
}