using System;
using Kernel.Federation.MetaData;

namespace Kernel.Federation.Protocols
{
    public interface IAuthnRequestBuilder
    {
        Uri BuildRedirectUri(AuthnRequestContext authnRequestContext);
    }
}