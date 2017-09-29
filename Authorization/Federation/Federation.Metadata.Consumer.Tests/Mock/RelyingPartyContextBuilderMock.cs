using System;
using Kernel.Federation.RelyingParty;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class RelyingPartyContextBuilderMock : ITenantContextBuilder
    {
        public TenantContext BuildRelyingPartyContext(string relyingPartyId)
        {
            var context = new TenantContext(relyingPartyId, "C:\\");

            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}