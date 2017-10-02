using System;
using Kernel.Federation.Protocols;

namespace Federation.Protocols.Bindings.HttpRedirect
{
    public class HttpRedirectContext : BindingContext
    {
        public HttpRedirectContext(AuthnRequestContext authnRequestContext) : base(authnRequestContext.RelyingState, authnRequestContext.Destination)
        {
            this.AuthnRequestContext = authnRequestContext;
        }
        
        public AuthnRequestContext AuthnRequestContext { get; set; }

        public override Uri GetDestinationUrl()
        {
            var url = String.Format("{0}?{1}", base.DestinationUri.AbsoluteUri, base.ClauseBuilder);
            return new Uri(url);
        }

    }
}