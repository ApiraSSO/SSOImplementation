using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Federation.Protocols.Request;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders
{
    internal class SignatureBuilder : ISamlClauseBuilder
    {
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly ICertificateManager _certificateManager;
        
        public SignatureBuilder(IFederationPartyContextBuilder federationPartyContextBuilder, ICertificateManager certificateManager)
        {
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._certificateManager = certificateManager;
        }
        public uint Order { get { return 2; } }

        public Task Build(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpRedirectContext = context as HttpRedirectContext;
            if (httpRedirectContext == null)
                throw new InvalidOperationException(String.Format("Binding context must be of type:{0}. It was: {1}", typeof(HttpRedirectContext).Name, context.GetType().Name));
            var federationParty = this._federationPartyContextBuilder.BuildContext(httpRedirectContext.AuthnRequestContext.FederationPartyId);
            var metadataContext = federationParty.MetadataContext;
            var entityDescriptor = metadataContext.EntityDesriptorConfiguration;
            var spDescriptor = entityDescriptor.SPSSODescriptors
                .First();
            var certContext = spDescriptor.KeyDescriptors.First(x => x.IsDefault && x.Use == Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Signing)
                .CertificateContext;
            if (spDescriptor.AuthenticationRequestsSigned)
                this.SignRequest(context.ClauseBuilder, certContext);
            return Task.CompletedTask;
        }

        internal void SignRequest(StringBuilder sb, CertificateContext certContext)
        {
            AuthnRequestHelper.AppendSignarureAlgorithm(sb);
            this.SignData(sb, certContext);
        }
        internal void SignData(StringBuilder sb, CertificateContext certContext)
        {
            var base64 = this._certificateManager.SignToBase64(sb.ToString(), certContext);
            var escaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(base64));
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.Signature, escaped);
        }
    }
}