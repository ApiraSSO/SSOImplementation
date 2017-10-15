using System;

namespace Kernel.Federation.Protocols
{
    public class AuthnContext
    {
        public AuthnContext(string authnContextType, Uri authnContextUri)
        {
            this.AuthnContextType = authnContextType;
            this.AuthnContextUri = authnContextUri;
        }
        public string AuthnContextType { get; }
        public Uri AuthnContextUri { get; }
    }
}