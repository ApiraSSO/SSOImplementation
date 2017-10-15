using System;

namespace Kernel.Federation.FederationPartner
{
    public interface IFederationPartyContextBuilder : IDisposable
    {
        FederationPartyConfiguration BuildContext(string federationPartyId);
    }
}