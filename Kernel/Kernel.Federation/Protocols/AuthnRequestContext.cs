using System;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination, string relyingPartyId)
        {
            this.Destination = destination;
            this.RelyingPartyId = relyingPartyId;
        }
        public string RelyingPartyId { get; }
        public Uri Destination { get; private set; }
    }
}