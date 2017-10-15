using System;
using Kernel.Federation.FederationPartner;

namespace Federation.Metadata.Consumer.Tests.Mock
{
    internal class FederationPartyContextBuilderMock : IFederationPartyContextBuilder
    {
        public FederationPartyConfiguration BuildContext(string federationPartyId)
        {
            var context = new FederationPartyConfiguration(federationPartyId, "C:\\");

            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}