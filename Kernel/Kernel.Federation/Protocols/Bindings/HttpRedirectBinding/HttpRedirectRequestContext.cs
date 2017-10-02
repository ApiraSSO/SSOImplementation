using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols.Bindings.HttpRedirectBinding
{
    public class HttpRedirectRequestContext : SamlRequestContext
    {
        public Func<Uri, Task> RequestHanlerAction { get; set; }
    }
}