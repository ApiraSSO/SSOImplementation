using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class BindingSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var redirectBinding = new Binding { Uri = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect", Name = "HTTP-Redirect" };
            context.Add<Binding>(redirectBinding);
            var postBinding = new Binding { Uri = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST", Name = "HTTP-POST" };
            context.Add<Binding>(postBinding);
            Seeder._cache.Add(Seeder.BindingsKey, new[] { redirectBinding, postBinding });
        }
    }
}