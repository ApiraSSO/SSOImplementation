using System;
using System.Collections.Generic;
using Kernel.Federation.FederationPartner;

namespace Kernel.Federation.Protocols
{
    public class AuthnRequestContext
    {
        public AuthnRequestContext(Uri destination, FederationPartyContext federationPartyContext)
        {
            this.FederationPartyContext = federationPartyContext;
            this.Destination = destination;
            this.RelyingState = new Dictionary<string, object> { {"federationPartyId", federationPartyContext.FederationPartyId } };
            this.Version = "2.0";
        }
        public string Version { get; set; }
        public IDictionary<string, object> RelyingState { get; }
        public Uri Destination { get; private set; }
        public FederationPartyContext FederationPartyContext { get; }
    }
}