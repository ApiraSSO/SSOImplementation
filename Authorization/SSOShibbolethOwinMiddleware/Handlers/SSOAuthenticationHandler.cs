using System;
using System.Linq;
using System.IdentityModel.Metadata;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Federation.Protocols.Request;
using Kernel.DependancyResolver;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Response;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;

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
        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            if (Request.Path == new PathString("/api/Account/SSOLogon"))
            {
                if (string.Equals(this.Request.Method, "POST", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(this.Request.ContentType) && (this.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) && this.Request.Body.CanRead))
                {
                    if (!this.Request.Body.CanSeek)
                    {
                        this._logger.WriteVerbose("Buffering request body");
                        MemoryStream memoryStream = new MemoryStream();
                        await this.Request.Body.CopyToAsync((Stream)memoryStream);
                        memoryStream.Seek(0L, SeekOrigin.Begin);
                        this.Request.Body = (Stream)memoryStream;
                    }
                    IFormCollection form = await this.Request.ReadFormAsync();
                    
                    var responseHandler = this._resolver.Resolve<IReponseHandler<Func<string, Task<ClaimsIdentity>>>>();
                    var identityDelegate = await responseHandler.Handle(() => form.ToDictionary(x => x.Key, v => form.Get(v.Key))as IDictionary<string, string>);
                    var identity = await identityDelegate(base.Options.AuthenticationType);
                    if(identity != null)
                        return new AuthenticationTicket(identity, new AuthenticationProperties());
                }
            }
            return null;
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

            var federationPartyId = FederationPartyIdentifierHelper.GetFederationPartyIdFromRequestOrDefault(Request.Context);
            if (this._configuration == null)
            {
                var configurationManager = this._resolver.Resolve<IConfigurationManager<MetadataBase>>();
                this._configuration = await configurationManager.GetConfigurationAsync(federationPartyId, new System.Threading.CancellationToken());
            }
            
            Uri signInUrl = null;
            var metadataType = this._configuration.GetType();
            var handlerType = typeof(IMetadataHandler<>).MakeGenericType(metadataType);
            var handler = this._resolver.Resolve(handlerType);
            var del = HandlerFactory.GetDelegateForIdpLocation(metadataType);
            signInUrl = del(handler, this._configuration, new Uri(Bindings.Http_Redirect));

            var requestContext = new AuthnRequestContext(signInUrl, federationPartyId);
            var redirectUriBuilder = this._resolver.Resolve<IAuthnRequestBuilder>();
            var redirectUri = await redirectUriBuilder.BuildRedirectUri(requestContext);
            
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