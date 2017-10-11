using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Federation.Protocols.Bindings.HttpRedirect;
using Kernel.DependancyResolver;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;
using Kernel.Federation.Protocols.Bindings.HttpRedirectBinding;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Shared.Federtion.Factories;

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

                    //ToDo: clean up get the associated request
                    var protocolFactory = this._resolver.Resolve<Func<string, IProtocolHandler>>();
                    var protocolHanlder = protocolFactory(Bindings.Http_Post);

                    var protocolContext = new SamlProtocolContext
                    {
                        ResponseContext = new HttpPostResponseContext
                        {
                            AuthenticationMethod = base.Options.AuthenticationType,
                            Form = form.ToDictionary(x => x.Key, v => form.Get(v.Key)) as IDictionary<string, string>
                        }
                        
                    };
                    await protocolHanlder.HandleResponse(protocolContext);
                    var responseContext = protocolContext.ResponseContext as HttpPostResponseContext;
                    var identity = responseContext.Result;
                    if (identity != null)
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

            //ToDo: sort this one in phase3 when implementing owin middleware. 
            //no need to have two methods in the handler. use GetDelegateForIdpDescriptors
            var locationDel = IdpMetadataHandlerFactory.GetDelegateForIdpLocation(metadataType);
            signInUrl = locationDel(handler, this._configuration, new Uri(Bindings.Http_Redirect));
            
            //the lines below are likely to do all what we need. 
            var idpDel = IdpMetadataHandlerFactory.GetDelegateForIdpDescriptors(this._configuration.GetType(), typeof(IdentityProviderSingleSignOnDescriptor));
            var idp = idpDel(handler, this._configuration).Cast<IdentityProviderSingleSignOnDescriptor>().First();

            var federationPartyContextBuilder = this._resolver.Resolve<IFederationPartyContextBuilder>();
            var federationContext = federationPartyContextBuilder.BuildContext(federationPartyId);

            var requestContext = new AuthnRequestContext(signInUrl, federationContext, idp.NameIdentifierFormats);
            var protocolContext = new SamlProtocolContext
            {
                RequestContext = new HttpRedirectRequestContext
                {
                    BindingContext = new HttpRedirectContext(requestContext),
                    RequestHanlerAction = redirectUri =>
                    {
                        this.Response.Redirect(redirectUri.AbsoluteUri);
                        return Task.CompletedTask;
                    }
                }
            };
            var protocolFactory = this._resolver.Resolve<Func<string, IProtocolHandler>>();
            var protocolHanlder = protocolFactory(Bindings.Http_Redirect);
            await protocolHanlder.HandleRequest(protocolContext);
        }
    }
}