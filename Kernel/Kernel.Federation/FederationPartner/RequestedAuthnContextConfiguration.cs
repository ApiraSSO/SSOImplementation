using System.Collections.Generic;
using Kernel.Federation.Protocols;

namespace Kernel.Federation.FederationPartner
{
    public class RequestedAuthnContextConfiguration
    {
        public RequestedAuthnContextConfiguration(string comparision)
        {
            this.Comparision = comparision;
            this.RequestedAuthnContexts = new List<AuthnContext>();
        }
        public string Comparision { get; }
        public ICollection<AuthnContext> RequestedAuthnContexts { get; }
    }
}