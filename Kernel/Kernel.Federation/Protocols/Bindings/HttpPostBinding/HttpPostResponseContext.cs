using System.Collections.Generic;
using System.Security.Claims;

namespace Kernel.Federation.Protocols.Bindings.HttpPostBinding
{
    public class HttpPostResponseContext : SamlResponseContext
    {
        public string AuthenticationMethod { get; set; }
        public IDictionary<string, string> Form { get; set; }
        public ClaimsIdentity Result { get; set; }
    }
}