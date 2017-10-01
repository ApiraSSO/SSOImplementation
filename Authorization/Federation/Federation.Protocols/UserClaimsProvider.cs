using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Federation.Protocols.Extensions;
using Kernel.Authentication.Claims;

namespace Federation.Protocols
{
    internal class UserClaimsProvider : IUserClaimsProvider<SecurityToken>
    {
        public Task<IDictionary<string, ClaimsIdentity>> GenerateUserIdentitiesAsync(SecurityToken user, IEnumerable<string> authenticationTypes)
        {
            var identity = Saml20AssertionExtensions.ToClaimsIdentity(((Saml2SecurityToken)user).Assertion, authenticationTypes.First());
            return Task.FromResult<IDictionary<string, ClaimsIdentity>>(new Dictionary<string, ClaimsIdentity>
            {
                {
                    authenticationTypes.First(), identity
                }
            });
        }
    }
}