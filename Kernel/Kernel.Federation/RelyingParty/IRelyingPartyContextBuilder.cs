using System;

namespace Kernel.Federation.RelyingParty
{
    public interface IRelyingPartyContextBuilder : IDisposable
    {
        RelyingPartyContext BuildRelyingPartyContext(string relyingPartyId);
    }
}