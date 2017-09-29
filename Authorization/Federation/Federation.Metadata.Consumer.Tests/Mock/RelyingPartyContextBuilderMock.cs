using System;
using Kernel.Federation.FederationPartner;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class RelyingPartyContextBuilderMock : IFederationPartnerContextBuilder
    {
        public FederationPartnerContext BuildContext(string relyingPartyId)
        {
            var context = new FederationPartnerContext(relyingPartyId, "C:\\");

            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}