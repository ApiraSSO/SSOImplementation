using System;
using System.Text;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.Protocols;
using Kernel.Federation.FederationPartner;
using Serialisation.Xml;

namespace Federation.Protocols.Request
{
    public class AuthnRequestBuilder : IAuthnRequestBuilder
    {
        private readonly ICertificateManager _certificateManager;
        private readonly IFederationPartnerContextBuilder _federationPartyContextBuilder;
        private readonly IXmlSerialiser _serialiser;

        public AuthnRequestBuilder(ICertificateManager certificateManager, IFederationPartnerContextBuilder federationPartyContextBuilder, IXmlSerialiser serialiser)
        {
            this._certificateManager = certificateManager;
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._serialiser = serialiser;
        }

        public Uri BuildRedirectUri(AuthnRequestContext authnRequestContext)
        {
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(authnRequestContext, this._federationPartyContextBuilder);

            var sb = new StringBuilder();
            var query = AuthnRequestHelper.SerialiseAndSign(authnRequest, authnRequestContext, this._serialiser, this._federationPartyContextBuilder, this._certificateManager);
            sb.AppendFormat("{0}?{1}", authnRequest.Destination, query);

            return new Uri(sb.ToString());
        }
    }
}