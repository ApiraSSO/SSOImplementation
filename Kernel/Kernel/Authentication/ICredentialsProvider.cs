using System.Net;

namespace Kernel.Authentication
{
    public interface ICredentialsProvider : ICredentials, ICredentialsByHost
    {
    }
}