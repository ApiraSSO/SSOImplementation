using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols.Response;
using System.Linq;
using Kernel.Cryptography.CertificateManagement;

namespace Federation.Protocols.Response
{
    internal class TokenHandlerConfigurationProvider : ITokenHandlerConfigurationProvider
    {
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        public TokenHandlerConfigurationProvider(IFederationPartyContextBuilder federationPartyContextBuilder)
        {
            this._federationPartyContextBuilder = federationPartyContextBuilder;
        }

        public void Configuration(ITokenHandler handler, string parnerId)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var saml2Handler = handler as System.IdentityModel.Tokens.Saml2SecurityTokenHandler;
            if (saml2Handler == null)
                throw new InvalidOperationException(String.Format("Expected type: {0} but was: {1}", typeof(System.IdentityModel.Tokens.Saml2SecurityTokenHandler).Name, handler.GetType().Name));

            var partnerContex = this._federationPartyContextBuilder.BuildContext(parnerId);
            var descriptor = partnerContex.MetadataContext.EntityDesriptorConfiguration.SPSSODescriptors.First();
            var cert = descriptor.KeyDescriptors.First(x => x.IsDefault && x.Use == Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Encryption);
            var x509CertificateContext = cert.CertificateContext as X509CertificateContext;
            var inner = new X509CertificateStoreTokenResolver(x509CertificateContext.StoreName, x509CertificateContext.StoreLocation);
            var tr = new IssuerTokenResolver(inner);

            saml2Handler.Configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerTokenResolver = tr,
                ServiceTokenResolver = tr

            };
        }
    }
}