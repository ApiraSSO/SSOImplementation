using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Federation.Protocols.Extensions;
using Kernel.Authentication.Claims;

namespace Federation.Protocols
{
    internal class ClaimsProvider : IUserClaimsProvider<Federation.Protocols.Response.Saml2SecurityTokenHandler>
    {
        public Task<IDictionary<string, ClaimsIdentity>> GenerateUserIdentitiesAsync(Federation.Protocols.Response.Saml2SecurityTokenHandler user, IEnumerable<string> authenticationTypes)
        {
            var identity = new Dictionary<string, ClaimsIdentity> { { authenticationTypes.First(), user.Claims.First() } };
            return Task.FromResult<IDictionary<string, ClaimsIdentity>>(identity);
        }
    }
}