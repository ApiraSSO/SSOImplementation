using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Federation.Protocols.Response;
using Kernel.Authentication.Claims;

namespace Federation.Protocols
{
    internal class ClaimsProvider : IUserClaimsProvider<SecurityTokenHandler>
    {
        public Task<IDictionary<string, ClaimsIdentity>> GenerateUserIdentitiesAsync(SecurityTokenHandler user, IEnumerable<string> authenticationTypes)
        {
            var identity = new Dictionary<string, ClaimsIdentity> { { authenticationTypes.First(), user.Claims.First() } };
            return Task.FromResult<IDictionary<string, ClaimsIdentity>>(identity);
        }
    }
}