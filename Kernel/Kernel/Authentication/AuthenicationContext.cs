using System.Security;
using Kernel.Extensions;

namespace Kernel.Authentication
{
    public class AuthenicationContext
    {
        public AuthenicationContext(string userName, string userEmail, ref string password)
        {
            this.UserName = userName;
            this.UserEmail = userEmail;
            this.Password = StringExtensions.ToSecureString(password);
            password = null;
        }
        public string UserName { get; private set; }
        public string UserEmail { get; private set; }
        public SecureString Password { get; private set; }
    }
}