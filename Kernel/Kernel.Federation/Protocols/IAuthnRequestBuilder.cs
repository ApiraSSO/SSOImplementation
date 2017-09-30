using System;
using System.Threading.Tasks;

namespace Kernel.Federation.Protocols
{
    public interface IAuthnRequestBuilder
    {
        Task<Uri> BuildRedirectUri(AuthnRequestContext authnRequestContext);
    }
}