using System;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination)
        {
            this.Destination = destination;
        }
        public Uri Destination { get; private set; }
    }
}
