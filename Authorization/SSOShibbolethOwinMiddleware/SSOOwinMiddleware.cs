using System;
using System.IdentityModel.Metadata;
using System.Net.Http;
using System.Net.Security;
using Kernel.DependancyResolver;
using Kernel.Federation.FederationPartner;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using SSOOwinMiddleware.Handlers;

namespace SSOOwinMiddleware
{
    internal class SSOOwinMiddleware : AuthenticationMiddleware<SSOAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly IDependencyResolver _resolver;
        public SSOOwinMiddleware(OwinMiddleware next, IAppBuilder app, SSOAuthenticationOptions options, IDependencyResolver resolver)
            : base(next, options)
        {
            this._resolver = resolver;
            this._logger = app.CreateLogger<SSOOwinMiddleware>();
            if (base.Options.BackchannelCertificateValidator == null)
            {
                base.Options.BackchannelCertificateValidator = this._resolver.Resolve<Kernel.Cryptography.Validation.IBackchannelCertificateValidator>();
            }

            if (string.IsNullOrWhiteSpace(this.Options.TokenValidationParameters.AuthenticationType))
                this.Options.TokenValidationParameters.AuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            if (this.Options.StateDataFormat == null)
                this.Options.StateDataFormat = (ISecureDataFormat<AuthenticationProperties>)new PropertiesDataFormat(app.CreateDataProtector(typeof(SSOOwinMiddleware).FullName, this.Options.AuthenticationType, "v1"));
            //if (this.Options.Notifications == null)
            //    this.Options.Notifications = new WsFederationAuthenticationNotifications();
            Uri result;
            if (!this.Options.CallbackPath.HasValue && !string.IsNullOrEmpty(this.Options.Wreply) && Uri.TryCreate(this.Options.Wreply, UriKind.Absolute, out result))
                this.Options.CallbackPath = PathString.FromUriComponent(result);
            if (this.Options.ConfigurationManager != null)
                return;
            if (this.Options.Configuration != null)
            { }//this.Options.ConfigurationManager = (IConfigurationManager<object>)new StaticConfigurationManager<object>(this.Options.Configuration);
            else
            {
                var httpClient = new HttpClient(SSOOwinMiddleware.ResolveHttpMessageHandler(this.Options))
                {
                    Timeout = this.Options.BackchannelTimeout,
                    MaxResponseContentBufferSize = 10485760L
                };

                this._resolver.RegisterFactory<Func<HttpClient>>(() =>
                {
                    return () => httpClient;
                }, Lifetime.Transient);

                //ToDo remove configuration manager from options
                //var configurationManager = this._resolver.Resolve<IConfigurationManager<MetadataBase>>();
                //this.Options.ConfigurationManager = configurationManager;
            }
        }
        
        protected override AuthenticationHandler<SSOAuthenticationOptions> CreateHandler()
        {
            return new SSOAuthenticationHandler(this._logger, this._resolver);
        }

        private static HttpMessageHandler ResolveHttpMessageHandler(SSOAuthenticationOptions options)
        {
            HttpMessageHandler httpMessageHandler = options.BackchannelHttpHandler ?? new WebRequestHandler();
            if (options.BackchannelCertificateValidator != null)
            {
                WebRequestHandler webRequestHandler = httpMessageHandler as WebRequestHandler;
                if (webRequestHandler == null)
                    throw new InvalidOperationException();
                webRequestHandler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(options.BackchannelCertificateValidator.Validate);
            }
            return httpMessageHandler;
        }
    }
}