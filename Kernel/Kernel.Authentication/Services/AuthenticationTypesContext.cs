using System.Collections.Generic;

namespace Kernel.Authentication.Services
{
    public class AuthenticationTypesContext : AuthenicationContext
    {
        public AuthenticationTypesContext(string userName, string userEmail, ref string password, IEnumerable<string> authenticationTypes)
            : base(userName, userEmail, ref password)
        {
            this.AuthenticationTypes = authenticationTypes;
        }

        public IEnumerable<string> AuthenticationTypes { get; private set; }
    }
}