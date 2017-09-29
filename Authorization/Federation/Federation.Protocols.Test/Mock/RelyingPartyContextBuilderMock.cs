using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.FederationPartner;

namespace Federation.Protocols.Test.Mock
{
    internal class RelyingPartyContextBuilderMock : IFederationPartnerContextBuilder
    {
        private InlineMetadataContextBuilder _inlineMetadataContextBuilder = new InlineMetadataContextBuilder();

        public FederationPartnerContext BuildContext(string relyingPartyId)
        {
            return new FederationPartnerContext("local", "https://dg-mfb/idp/shibboleth")
            {
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}