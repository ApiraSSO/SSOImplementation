using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.FederationPartner;

namespace Federation.Protocols.Test.Mock
{
    internal class FederationPartyContextBuilderMock : IFederationPartyContextBuilder
    {
        private InlineMetadataContextBuilder _inlineMetadataContextBuilder = new InlineMetadataContextBuilder();

        public FederationPartyContext BuildContext(string federationPartyId)
        {
            return new FederationPartyContext("local", "https://dg-mfb/idp/shibboleth")
            {
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}