using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.EntityFramework.IdentityProvider.Managers;
using AspNet.EntityFramework.IdentityProvider.Models;
using Kernel.Authentication;
using Kernel.Authentication.Claims;
using Kernel.Authentication.Services;

namespace AspNet.EntityFramework.IdentityProvider
{
    internal class IdentityProviderService : IIdentityProviderService
    {
        private readonly ApplicationUserManager _manager;
        private readonly IUserClaimsProvider<ApplicationUser> _claimsProvider;

        public IdentityProviderService(ApplicationUserManager manager, IUserClaimsProvider<ApplicationUser> claimsProvider)
        {
            this._manager = manager;
            this._claimsProvider = claimsProvider;
        }
        public async Task<AuthenticationResult> AuthenticateUser(AuthenticationTypesContext context)
        {
            IDictionary<string, ClaimsIdentity> identity = new Dictionary<string, ClaimsIdentity>();
            AuthenticationResults authenticationResult = AuthenticationResults.FailInvalidCredentials;
            var user = await this._manager.FindAsync(context.UserName, context.Password);
            if(user != null)
            {
                authenticationResult = AuthenticationResults.Success;
                identity = await this._claimsProvider.GenerateUserIdentitiesAsync(user, context.AuthenticationTypes);
            }
            return new AuthenticationResult(authenticationResult, identity);
        }
    }
}