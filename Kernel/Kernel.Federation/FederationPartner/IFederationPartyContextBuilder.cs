using System;

namespace Kernel.Federation.FederationPartner
{
    public interface IFederationPartyContextBuilder : IDisposable
    {
        FederationPartyContext BuildContext(string federationPartyId);
    }
}