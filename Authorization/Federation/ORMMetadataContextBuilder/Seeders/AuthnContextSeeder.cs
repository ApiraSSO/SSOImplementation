using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;
using Shared.Federtion.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class AuthnContextSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var authnContext = new SamlAutnContext
            {
                Value = "urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport",
                RefType = AuthnContextType.AuthnContextClassRef
            };

            context.Add<SamlAutnContext>(authnContext);

            Seeder._cache.Add(Seeder.SamlAutnContextKey, new[] { authnContext });
        }
    }
}