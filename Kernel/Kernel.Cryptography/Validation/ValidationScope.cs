using System;

namespace Kernel.Cryptography.Validation
{
    [Flags]
    public enum ValidationScope
    {
        Certificate = 1,
        BackchannelCertificate= 2
    }
}