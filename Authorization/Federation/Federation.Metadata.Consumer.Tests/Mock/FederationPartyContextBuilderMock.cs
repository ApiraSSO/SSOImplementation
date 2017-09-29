using System;
using Kernel.Federation.FederationPartner;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class FederationPartyContextBuilderMock : IFederationPartnerContextBuilder
    {
        public FederationPartnerContext BuildContext(string federationPartyId)
        {
            var context = new FederationPartnerContext(federationPartyId, "C:\\");

            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}