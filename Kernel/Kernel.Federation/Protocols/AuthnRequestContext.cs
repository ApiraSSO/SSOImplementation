using System;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination, string federationPartyId)
        {
            this.Destination = destination;
            this.FederationPartyId = federationPartyId;
            this.Version = "2.0";
        }
        public string Version { get; set; }
        public string FederationPartyId { get; }
        public Uri Destination { get; private set; }
    }
}