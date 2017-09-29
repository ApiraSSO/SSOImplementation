using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.RelyingParty;

namespace Federation.Protocols.Test.Mock
{
    internal class RelyingPartyContextBuilderMock : ITenantContextBuilder
    {
        private InlineMetadataContextBuilder _inlineMetadataContextBuilder = new InlineMetadataContextBuilder();

        public TenantContext BuildRelyingPartyContext(string relyingPartyId)
        {
            return new TenantContext("local", "https://dg-mfb/idp/shibboleth")
            {
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}