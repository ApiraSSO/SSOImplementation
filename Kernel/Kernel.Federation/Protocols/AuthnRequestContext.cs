using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel.Federation.FederationConfiguration;
using Kernel.Federation.MetaData;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(IConfiguration configuration, Uri destination)
        {
            this.Destination = destination;
            this.Configuration = configuration;
        }
        public Uri Destination { get; private set; }
        public IConfiguration Configuration { get; private set; }
    }
}
