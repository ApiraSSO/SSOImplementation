using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kernel.Authentication.Claims;

namespace Federation.Protocols
{
    internal class UserClaimsProvider : IUserClaimsProvider<SecurityToken>
    {
        public Task<IDictionary<string, ClaimsIdentity>> GenerateUserIdentitiesAsync(SecurityToken user, IEnumerable<string> authenticationTypes)
        {
            return Task.FromResult<IDictionary<string, ClaimsIdentity>>(new Dictionary<string, ClaimsIdentity>());
        }
    }
}