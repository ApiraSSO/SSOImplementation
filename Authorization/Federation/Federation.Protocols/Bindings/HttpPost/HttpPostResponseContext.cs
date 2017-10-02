using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Federation.Protocols.Bindings.HttpPost
{
    public class HttpPostResponseContext
    {
        public Func<IDictionary<string, string>> Form { get; set; }
        public Func<string, Task<ClaimsIdentity>> Result { get; set; }
    }
}