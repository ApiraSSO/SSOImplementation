using System;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.Protocols;
using Kernel.Federation.RelyingParty;
using Serialisation.Xml;

namespace Federation.Protocols.Request
{
    public class AuthnRequestBuilder : IAuthnRequestBuilder
    {
        private readonly ICertificateManager _certificateManager;
        private readonly IRelyingPartyContextBuilder _relyingPartyContextBuilder;
        private readonly IXmlSerialiser _serialiser;

        public AuthnRequestBuilder(ICertificateManager certificateManager, IRelyingPartyContextBuilder relyingPartyContextBuilder, IXmlSerialiser serialiser)
        {
            this._certificateManager = certificateManager;
            this._relyingPartyContextBuilder = relyingPartyContextBuilder;
            this._serialiser = serialiser;
        }

        public Uri BuildRedirectUri(AuthnRequestContext authnRequestContext)
        {
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, this._relyingPartyContextBuilder);

            var sb = new StringBuilder();
            var query = AuthnRequestHelper.SerialiseAndSign(authnRequest, authnRequestContext, this._serialiser, this._relyingPartyContextBuilder, this._certificateManager);
            sb.AppendFormat("{0}?{1}", authnRequest.Destination, query);

            return new Uri(sb.ToString());
        }
    }
}