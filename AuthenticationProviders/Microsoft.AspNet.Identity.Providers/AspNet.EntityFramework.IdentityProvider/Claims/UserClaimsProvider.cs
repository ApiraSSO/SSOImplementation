using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using AspNet.EntityFramework.IdentityProvider.Managers;
using AspNet.EntityFramework.IdentityProvider.Models;
using Kernel.Authentication.Claims;
using Kernel.DependancyResolver;

namespace AspNet.EntityFramework.IdentityProvider.Claims
{
    internal class UserClaimsProvider : IUserClaimsProvider<ApplicationUser>
    {
        private readonly IDependencyResolver _dependencyResolver;

        public UserClaimsProvider(IDependencyResolver dependencyResolver)
        {
            this._dependencyResolver = dependencyResolver;
        }
        public async Task<IDictionary<string, ClaimsIdentity>> GenerateUserIdentitiesAsync(ApplicationUser user, IEnumerable<string> authenticationTypes)
        {
           
            var manager = _dependencyResolver.Resolve<ApplicationUserManager>();
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentities = authenticationTypes.Aggregate(new Dictionary<string, ClaimsIdentity>(),
                (d, authenticationType) =>
                {
                    var id = manager.CreateIdentityAsync(user, authenticationType).Result;
                    d.Add(authenticationType, id);
                    return d;
                });
            // Add custom user claims here
            
            return userIdentities;
        }
    }
}