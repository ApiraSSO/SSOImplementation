using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using System;

namespace Federation.Protocols.Test.Mock
{
    internal class FederationPartyContextBuilderMock : IFederationPartyContextBuilder
    {
        private InlineMetadataContextBuilder _inlineMetadataContextBuilder = new InlineMetadataContextBuilder();

        public FederationPartyConfiguration BuildContext(string federationPartyId)
        {
            return this.BuildContext(federationPartyId, NameIdentifierFormats.Unspecified);
        }

        public FederationPartyConfiguration BuildContext(string federationPartyId, string defaultNameIdFormat)
        {
            return new FederationPartyConfiguration("local", "https://dg-mfb/idp/shibboleth")
            {
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}