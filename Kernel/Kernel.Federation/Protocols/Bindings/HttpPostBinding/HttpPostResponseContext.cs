using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols.Bindings.HttpPostBinding
{
    public class HttpPostResponseContext : SamlResponseContext
    {
        public Func<IDictionary<string, string>> Form { get; set; }
        public Func<string, Task<ClaimsIdentity>> Result { get; set; }
    }
}