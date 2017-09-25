using System;

namespace Kernel.Federation.MetaData.Configuration.Cryptography
{
    [Flags]
    public enum KeyTarget
    {
        MetaData = 1,
        Request = 2
    }
}