using System;
using Kernel.Federation.FederationPartner;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination, FederationPartyContext federationPartyContext)
        {
            this.FederationPartyContext = federationPartyContext;
            this.Destination = destination;
            this.RelyingState = federationPartyContext.FederationPartyId;
            this.Version = "2.0";
        }
        public string Version { get; set; }
        public string RelyingState { get; set; }
        public Uri Destination { get; private set; }
        public FederationPartyContext FederationPartyContext { get; }
    }
}