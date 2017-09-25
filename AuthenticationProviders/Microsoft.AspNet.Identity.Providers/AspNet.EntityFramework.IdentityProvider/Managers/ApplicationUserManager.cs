using System;
using System.Security;
using System.Threading.Tasks;
using AspNet.EntityFramework.IdentityProvider.Models;
using Kernel.Extensions;
using Microsoft.AspNet.Identity;

namespace AspNet.EntityFramework.IdentityProvider.Managers
{
    internal class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(
            IUserStore<ApplicationUser> store,
            IUserTokenProvider<ApplicationUser, string> tokenProvider,
            IIdentityValidator<string> passwordValidator,
            Func<UserManager<ApplicationUser>, IIdentityValidator<ApplicationUser>> userValidatorFactory)
            : base(store)
        {
            base.UserTokenProvider = tokenProvider;
            base.PasswordValidator = passwordValidator;
            base.UserValidator = userValidatorFactory(this);
        }

        public Task<ApplicationUser> FindAsync(string userName, SecureString password)
        {
            var psw = StringExtensions.ToInsecureString(password);
            var user =  base.FindAsync(userName, psw);
            return user;
        }
    }
}