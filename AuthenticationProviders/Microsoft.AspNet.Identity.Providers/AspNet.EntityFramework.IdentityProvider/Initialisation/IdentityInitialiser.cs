using System;
using System.Threading.Tasks;
using AspNet.EntityFramework.IdentityProvider.Claims;
using AspNet.EntityFramework.IdentityProvider.Managers;
using AspNet.EntityFramework.IdentityProvider.Models;
using Kernel.DependancyResolver;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shared.Initialisation;

namespace AspNet.EntityFramework.IdentityProvider.Initialisation
{
    public class IdentityInitialiser : Initialiser
    {
        public override byte Order
        {
            get { return 1; }
        }

        protected override Task InitialiseInternal(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterType<ApplicationUserManager>(Lifetime.Transient);
            dependencyResolver.RegisterType<IdentityProviderService>(Lifetime.Transient);
            dependencyResolver.RegisterType<UserClaimsProvider>(Lifetime.Transient);

            dependencyResolver.RegisterFactory<IIdentityValidator<string>>(() =>
            new PasswordValidator
            {
                RequireDigit = true,
                RequiredLength = 6,
                RequireLowercase = true,
                RequireNonLetterOrDigit = true,
                RequireUppercase = true
            }, Lifetime.Transient);

            dependencyResolver.RegisterFactory<IUserStore<ApplicationUser>>(() =>
            new UserStore<ApplicationUser>(new ApplicationDbContext()), Lifetime.Transient);

            dependencyResolver.RegisterFactory<Func<UserManager<ApplicationUser>, IIdentityValidator<ApplicationUser>>>(() =>
            m => new UserValidator<ApplicationUser>(m)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            }, Lifetime.Transient);

            return Task.CompletedTask;
        }
    }
}