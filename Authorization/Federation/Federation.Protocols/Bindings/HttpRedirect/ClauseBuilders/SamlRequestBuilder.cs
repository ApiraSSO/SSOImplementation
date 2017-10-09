using System;
using System.Text;
using System.Threading.Tasks;
using Federation.Protocols.Request;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols;
using Serialisation.Xml;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders
{
    internal class SamlRequestBuilder : ISamlClauseBuilder
    {
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly IXmlSerialiser _serialiser;
        private readonly IMessageEncoding _messageEncoding;
        public SamlRequestBuilder(IFederationPartyContextBuilder federationPartyContextBuilder, IXmlSerialiser serialiser, IMessageEncoding messageEncoding)
        {
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._serialiser = serialiser;
            this._messageEncoding = messageEncoding;
        }
        public uint Order { get { return 0; } }

        public async Task Build(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpRedirectContext = context as HttpRedirectContext;
            if (httpRedirectContext == null)
                throw new InvalidOperationException(String.Format("Binding context must be of type:{0}. It was: {1}", typeof(HttpRedirectContext).Name, context.GetType().Name));
            var authnRequest = AuthnRequestHelper.BuildAuthnRequest(httpRedirectContext.AuthnRequestContext, this._federationPartyContextBuilder);

            var serialised = AuthnRequestHelper.Serialise(authnRequest, this._serialiser);
            await this.AppendRequest(context.ClauseBuilder, serialised);
        }

        internal async Task AppendRequest(StringBuilder builder, string request)
        {
            var compressed = await this._messageEncoding.EncodeMessage<string>(request);
            var encodedEscaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(compressed));
            builder.AppendFormat("{0}={1}", HttpRedirectBindingConstants.SamlRequest, encodedEscaped);
        }
    }
}