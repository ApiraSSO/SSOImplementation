using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class TokenHandlerConfigurationProvider : ITokenHandlerConfigurationProvider
    {
        public void Configuration(ITokenHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var saml2Handler = handler as System.IdentityModel.Tokens.Saml2SecurityTokenHandler;
            if (saml2Handler == null)
                throw new InvalidOperationException(String.Format("Expected type: {0} but was: {1}", typeof(System.IdentityModel.Tokens.Saml2SecurityTokenHandler).Name, handler.GetType().Name));

            var inner = new X509CertificateStoreTokenResolver("TestCertStore", StoreLocation.LocalMachine);
            var tr = new IssuerTokenResolver(inner);

            saml2Handler.Configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerTokenResolver = tr,
                ServiceTokenResolver = tr

            };
        }
    }
}