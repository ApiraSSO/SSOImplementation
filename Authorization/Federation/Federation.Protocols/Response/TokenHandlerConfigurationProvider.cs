using System;
using System.IdentityModel.Tokens;
using System.Linq;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class TokenHandlerConfigurationProvider : ITokenHandlerConfigurationProvider
    {
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        public TokenHandlerConfigurationProvider(IFederationPartyContextBuilder federationPartyContextBuilder)
        {
            this._federationPartyContextBuilder = federationPartyContextBuilder;
        }

        public void Configuration(ITokenHandler handler, string partnerId)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var saml2Handler = handler as System.IdentityModel.Tokens.Saml2SecurityTokenHandler;
            if (saml2Handler == null)
                throw new InvalidOperationException(String.Format("Expected type: {0} but was: {1}", typeof(System.IdentityModel.Tokens.Saml2SecurityTokenHandler).Name, handler.GetType().Name));

            var partnerContex = this._federationPartyContextBuilder.BuildContext(partnerId);
            var descriptor = partnerContex.MetadataContext.EntityDesriptorConfiguration.SPSSODescriptors.First();
            var cert = descriptor.KeyDescriptors.First(x => x.IsDefault && x.Use == Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Encryption);
            if (cert.CertificateContext == null)
                throw new ArgumentNullException("certificate contexr");

            var x509CertificateContext = cert.CertificateContext as X509CertificateContext;
            if (x509CertificateContext == null)
                throw new InvalidOperationException(String.Format("Expected certificate context of type: {0} but it was:{1}", typeof(X509CertificateContext).Name, cert.CertificateContext.GetType()));

            var inner = new X509CertificateStoreTokenResolver(x509CertificateContext.StoreName, x509CertificateContext.StoreLocation);
            var tokenResolver = new IssuerTokenResolver(inner);

            saml2Handler.Configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerTokenResolver = tokenResolver,
                ServiceTokenResolver = tokenResolver

            };
        }
    }
}