using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Cryptography.DataProtection
{
    public interface IDataProtector
    {
        byte[] Protect(byte[] userData);

        byte[] Unprotect(byte[] protectedData);
    }
}