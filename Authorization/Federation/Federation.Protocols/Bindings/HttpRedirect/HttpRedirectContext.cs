using Kernel.Federation.Protocols;

namespace Federation.Protocols.Bindings.HttpRedirect
{
    public class HttpRedirectContext : BindingContext
    {
        public HttpRedirectContext(AuthnRequestContext authnRequestContext) : base(authnRequestContext.RelyingState)
        {
            this.AuthnRequestContext = authnRequestContext;
        }
        
        public AuthnRequestContext AuthnRequestContext { get; set; }
        
    }
}