using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public class SamlProtocolContext
    {
        public BindingContext BindingContext { get; set; }
        public Func<Uri, Task> RequestHanlerAction { get; set; }

        public object HttpPostResponseContext { get; set; }
    }
}
