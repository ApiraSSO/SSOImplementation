using System;
using Kernel.Federation.RelyingParty;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class RelyingPartyContextBuilderMock : IRelyingPartyContextBuilder
    {
        public RelyingPartyContext BuildRelyingPartyContext(string relyingPartyId)
        {
            var context = new RelyingPartyContext(relyingPartyId, "C:\\");

            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}