using System;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Federation.Protocols.Request;
using Kernel.DependancyResolver;
using Kernel.Federation.Protocols;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace SSOOwinMiddleware.Handlers
{
    internal class SSOAuthenticationHandler : AuthenticationHandler<SSOAuthenticationOptions>
    {
        private const string HandledResponse = "HandledResponse";
        private readonly ILogger _logger;
        private MetadataBase _configuration;
        private readonly IDependencyResolver _resolver;

        public SSOAuthenticationHandler(ILogger logger, IDependencyResolver resolver)
        {
            this._resolver = resolver;
            this._logger = logger;
        }

        public override Task<bool> InvokeAsync()
        {
            if (!this.Options.SSOPath.HasValue || base.Request.Path != this.Options.SSOPath)
                return base.InvokeAsync();
            Context.Authentication.Challenge("Shibboleth");
            return Task.FromResult(true);
            
        }
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            return Task.FromResult((AuthenticationTicket)null);
        }
        protected override async Task ApplyResponseChallengeAsync()
        {
            if (this.Response.StatusCode != 401)
                return;

            var challenge = this.Helper.LookupChallenge(this.Options.AuthenticationType, this.Options.AuthenticationMode);
            if (challenge == null)
                return;

            if (!this.Options.SSOPath.HasValue || base.Request.Path != this.Options.SSOPath)
                return;

            //ToDo: shoudn't need those. The tests don't so probably reletated to IIS express etc
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var relyingPartyId = base.Options.Wtrealm;
            if (this._configuration == null)
                this._configuration = await this.Options.ConfigurationManager.GetConfigurationAsync(relyingPartyId, new System.Threading.CancellationToken());
            
            Uri signInUrl = null;

            var entitiesDescriptors = this._configuration as EntitiesDescriptor;
            if (entitiesDescriptors != null)
            {
                var idDescpritor = entitiesDescriptors.ChildEntities.SelectMany(x => x.RoleDescriptors)
                    .First(x => x.GetType() == typeof(IdentityProviderSingleSignOnDescriptor)) as IdentityProviderSingleSignOnDescriptor;
                signInUrl = idDescpritor.SingleSignOnServices.FirstOrDefault(x => x.Binding == new Uri("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect"))
                    .Location;
            }

            var entitityDescriptor = this._configuration as EntityDescriptor;
            if (entitityDescriptor != null)
            {
                var idDescpritor = entitityDescriptor.RoleDescriptors.Select(x => x)
                    .First(x => x.GetType() == typeof(IdentityProviderSingleSignOnDescriptor)) as IdentityProviderSingleSignOnDescriptor;
                signInUrl = idDescpritor.SingleSignOnServices.FirstOrDefault(x => x.Binding == new Uri("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect"))
                    .Location;
            }

            var requestContext = new AuthnRequestContext(null, signInUrl);
            var redirectUriBuilder = this._resolver.Resolve<AuthnRequestBuilder>();
            var redirectUri = redirectUriBuilder.BuildRedirectUri(requestContext);
            
            //string baseUri = this.Request.Scheme + Uri.SchemeDelimiter + (object)this.Request.Host + (object)this.Request.PathBase;
            //string currentUri = baseUri + (object)this.Request.Path + (object)this.Request.QueryString;
            //AuthenticationProperties properties = challenge.Properties;
            //if (string.IsNullOrEmpty(properties.RedirectUri))
            //    properties.RedirectUri = currentUri;
            //WsFederationMessage federationMessage = new WsFederationMessage();
            //federationMessage.IssuerAddress = this._configuration.TokenEndpoint ?? string.Empty;
            //federationMessage.Wtrealm = this.Options.Wtrealm;
            //federationMessage.Wctx = "WsFedOwinState=" + Uri.EscapeDataString(this.Options.StateDataFormat.Protect(properties));
            //federationMessage.Wa = "wsignin1.0";
            //WsFederationMessage wsFederationMessage = federationMessage;
            //if (!string.IsNullOrWhiteSpace(this.Options.Wreply))
            //    wsFederationMessage.Wreply = this.Options.Wreply;
            //RedirectToIdentityProviderNotification<WsFederationMessage, WsFederationAuthenticationOptions> notification = new RedirectToIdentityProviderNotification<WsFederationMessage, WsFederationAuthenticationOptions>(this.Context, this.Options)
            //{
            //    ProtocolMessage = wsFederationMessage
            //};
            //await this.Options.Notifications.RedirectToIdentityProvider(notification);
            //if (notification.HandledResponse)
            //    return;
            //string signInUrl = notification.ProtocolMessage.CreateSignInUrl();
            //if (!Uri.IsWellFormedUriString(signInUrl, UriKind.Absolute))
            //    this._logger.WriteWarning("The sign-in redirect URI is malformed: " + signInUrl);
            this.Response.Redirect(redirectUri.AbsoluteUri);
        }

    }
}