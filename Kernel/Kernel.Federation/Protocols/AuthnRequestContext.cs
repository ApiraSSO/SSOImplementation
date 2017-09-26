using System;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination, string relyingPartyId)
        {
            this.Destination = destination;
            this.RelyingPartyId = relyingPartyId;
            this.Version = "2.0";
        }
        public string Version { get; set; }
        public string RelyingPartyId { get; }
        public Uri Destination { get; private set; }
    }
}