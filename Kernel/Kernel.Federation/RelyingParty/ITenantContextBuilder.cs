using System;

namespace Kernel.Federation.RelyingParty
{
    public interface ITenantContextBuilder : IDisposable
    {
        TenantContext BuildRelyingPartyContext(string relyingPartyId);
    }
}