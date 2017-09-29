using System;

namespace Kernel.Federation.Tenant
{
    public interface ITenantContextBuilder : IDisposable
    {
        TenantContext BuildRelyingPartyContext(string relyingPartyId);
    }
}