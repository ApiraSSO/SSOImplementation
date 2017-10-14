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

        public FederationPartyContext BuildContext(string federationPartyId)
        {
            return this.BuildContext(federationPartyId, NameIdentifierFormats.Unspecified);
        }

        public FederationPartyContext BuildContext(string federationPartyId, string defaultNameIdFormat)
        {
            return new FederationPartyContext("local", "https://nadim/idp/shibboleth")
            {
                DefaultNameIdFormat = new Uri(defaultNameIdFormat),
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}