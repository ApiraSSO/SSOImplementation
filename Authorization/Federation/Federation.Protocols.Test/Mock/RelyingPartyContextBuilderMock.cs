using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.RelyingParty;

namespace Federation.Protocols.Test.Mock
{
    internal class RelyingPartyContextBuilderMock : IRelyingPartyContextBuilder
    {
        private InlineMetadataContextBuilder _inlineMetadataContextBuilder = new InlineMetadataContextBuilder();

        public RelyingPartyContext BuildRelyingPartyContext(string relyingPartyId)
        {
            return new RelyingPartyContext("local", "https://dg-mfb/idp/shibboleth")
            {
                MetadataContext = this._inlineMetadataContextBuilder.BuildContext(new MetadataGenerateRequest(MetadataType.SP, "local")),
            };
        }

        public void Dispose()
        {
        }
    }
}