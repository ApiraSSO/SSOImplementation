using Kernel.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNet.EntityFramework.IdentityProvider.Models
{
    internal class ApplicationUser : IdentityUser, IHasID<string>
    {
    }
}