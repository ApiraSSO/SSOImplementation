using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Cryptography.Validation;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class TokenHandlerConfigurationProvider : ITokenConfigurationProvider<SecurityTokenHandlerConfiguration>, ITokenHandlerConfigurationProvider
    {
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly ICertificateValidator _certificateValidator;
        public TokenHandlerConfigurationProvider(IFederationPartyContextBuilder federationPartyContextBuilder, ICertificateValidator certificateValidator)
        {
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._certificateValidator = certificateValidator;
        }

        public void Configuration(ITokenHandler handler, string partnerId)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var saml2Handler = handler as System.IdentityModel.Tokens.Saml2SecurityTokenHandler;
            if (saml2Handler == null)
                throw new InvalidOperationException(String.Format("Expected type: {0} but was: {1}", typeof(System.IdentityModel.Tokens.Saml2SecurityTokenHandler).Name, handler.GetType().Name));

            this._certificateValidator.SetFederationPartyId(partnerId);

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

            var configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerTokenResolver = tokenResolver,
                ServiceTokenResolver = inner,
                AudienceRestriction = new AudienceRestriction(AudienceUriMode.Always),
                CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom,
                CertificateValidator = (X509CertificateValidator)this._certificateValidator
            };

            saml2Handler.SamlSecurityTokenRequirement.CertificateValidator = configuration.CertificateValidator;
            configuration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(partnerContex.MetadataContext.EntityId));

            saml2Handler.Configuration = configuration;
            
            //ToDo: sort this one
            
            if(!((ConfigurationBasedIssuerNameRegistry)saml2Handler.Configuration.IssuerNameRegistry).ConfiguredTrustedIssuers.ContainsKey("953926B57F873960222A2F1C4002FAF9636B8D47"))
                ((ConfigurationBasedIssuerNameRegistry)saml2Handler.Configuration.IssuerNameRegistry).AddTrustedIssuer("953926B57F873960222A2F1C4002FAF9636B8D47", "https://idp.testshib.org/idp/shibboleth");
        }

        public SecurityTokenHandlerConfiguration GetConfiguration(string partnerId)
        {
            this._certificateValidator.SetFederationPartyId(partnerId);

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

            var configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerTokenResolver = tokenResolver,
                ServiceTokenResolver = inner,
                AudienceRestriction = new AudienceRestriction(AudienceUriMode.Always),
                CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom,
                CertificateValidator = (X509CertificateValidator)this._certificateValidator
            };
            
            configuration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(partnerContex.MetadataContext.EntityId));

            //ToDo: sort this one

            if (!((ConfigurationBasedIssuerNameRegistry)configuration.IssuerNameRegistry).ConfiguredTrustedIssuers.ContainsKey("953926B57F873960222A2F1C4002FAF9636B8D47"))
                ((ConfigurationBasedIssuerNameRegistry)configuration.IssuerNameRegistry).AddTrustedIssuer("953926B57F873960222A2F1C4002FAF9636B8D47", "https://idp.testshib.org/idp/shibboleth");
            return configuration;
        }

        public SecurityTokenHandlerConfiguration GetTrustedIssuersConfiguration()
        {
            var configuration = new SecurityTokenHandlerConfiguration
            {
               
            };
            
            //ToDo: sort this one

            if (!((ConfigurationBasedIssuerNameRegistry)configuration.IssuerNameRegistry).ConfiguredTrustedIssuers.ContainsKey("953926B57F873960222A2F1C4002FAF9636B8D47"))
                ((ConfigurationBasedIssuerNameRegistry)configuration.IssuerNameRegistry).AddTrustedIssuer("953926B57F873960222A2F1C4002FAF9636B8D47", "https://idp.testshib.org/idp/shibboleth");
            return configuration;
        }
    }
}