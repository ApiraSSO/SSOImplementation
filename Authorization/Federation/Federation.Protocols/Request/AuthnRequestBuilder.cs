using System;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.Protocols;
using Kernel.Federation.FederationPartner;
using Serialisation.Xml;
using Kernel.Compression;
using System.Threading.Tasks;
using Federation.Protocols.Bindings.HttpRedirect;

namespace Federation.Protocols.Request
{
    public class AuthnRequestBuilder : IAuthnRequestBuilder
    {
        private readonly ICertificateManager _certificateManager;
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly IXmlSerialiser _serialiser;
        private readonly ICompression _compression;

        public AuthnRequestBuilder(ICertificateManager certificateManager, IFederationPartyContextBuilder federationPartyContextBuilder, IXmlSerialiser serialiser, ICompression compression)
        {
            this._certificateManager = certificateManager;
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._serialiser = serialiser;
            this._compression = compression;
        }

        public async Task<Uri> BuildRedirectUri(AuthnRequestContext authnRequestContext)
        {
            var bindingHandler = new HttpRedirectBindingHandler();
            var contex = new HttpRedirectContext(authnRequestContext);
            await bindingHandler.BuildRequest(contex);
            var url = contex.GetDestinationUrl();
            return url;
            //var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, this._federationPartyContextBuilder);

            //var sb = new StringBuilder();
            ////var query = await AuthnRequestHelper.SerialiseAndSign(authnRequest, authnRequestContext, this._serialiser, this._federationPartyContextBuilder, this._certificateManager, this._compression);
            ////sb.AppendFormat("{0}?{1}", authnRequest.Destination, query);
            //sb.AppendFormat("{0}?{1}", authnRequestContext.Destination, contex.ClauseBuilder.ToString());

            //return new Uri(sb.ToString());
        }
    }
}