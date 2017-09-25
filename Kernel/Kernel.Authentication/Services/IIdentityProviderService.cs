using System.Threading.Tasks;

namespace Kernel.Authentication.Services
{
    public interface IIdentityProviderService
    {
        Task<AuthenticationResult> AuthenticateUser(AuthenticationTypesContext context);
    }
}