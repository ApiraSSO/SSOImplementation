using System.Collections.Generic;
using System.Security.Claims;

namespace Kernel.Authentication.Services
{
    public class AuthenticationResult
    {
        public AuthenticationResult(AuthenticationResults authenticationResult, IDictionary<string, ClaimsIdentity> identities)
        {
            this.Result = authenticationResult;
            this.Identities = identities;
        }

        public IDictionary<string, ClaimsIdentity> Identities { get; }
        public AuthenticationResults Result { get; }
    }
}