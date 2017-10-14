using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.Protocols;
using Kernel.Logging;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Bindings.HttpRedirect.ClauseBuilders
{
    internal class SignatureBuilder : ISamlClauseBuilder
    {
        private readonly ICertificateManager _certificateManager;
        readonly ILogProvider _logProvider;

        public SignatureBuilder(ICertificateManager certificateManager, ILogProvider logProvider)
        {
            this._certificateManager = certificateManager;
            this._logProvider = logProvider;
        }
        public uint Order { get { return 2; } }

        public Task Build(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var httpRedirectContext = context as HttpRedirectContext;
            if (httpRedirectContext == null)
                throw new InvalidOperationException(String.Format("Binding context must be of type:{0}. It was: {1}", typeof(HttpRedirectContext).Name, context.GetType().Name));
            var federationParty = httpRedirectContext.AuthnRequestContext.FederationPartyContext;
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
            this.AppendSignarureAlgorithm(sb);
            this.SignData(sb, certContext);
        }
        internal void SignData(StringBuilder sb, CertificateContext certContext)
        {
            this._logProvider.LogMessage(String.Format("Signing request with certificate from context: {0}", certContext.ToString()));
            var base64 = this._certificateManager.SignToBase64(sb.ToString(), certContext);
            var escaped = Uri.EscapeDataString(Helper.UpperCaseUrlEncode(base64));
            sb.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.Signature, escaped);
        }

        internal void AppendSignarureAlgorithm(StringBuilder builder)
        {
            builder.AppendFormat("&{0}={1}", HttpRedirectBindingConstants.SigAlg, Uri.EscapeDataString(SignedXml.XmlDsigRSASHA1Url));
        }
    }
}