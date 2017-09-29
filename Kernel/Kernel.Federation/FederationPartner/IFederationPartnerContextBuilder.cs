using System;

namespace Kernel.Federation.FederationPartner
{
    public interface IFederationPartnerContextBuilder : IDisposable
    {
        FederationPartnerContext BuildContext(string relyingPartyId);
    }
}