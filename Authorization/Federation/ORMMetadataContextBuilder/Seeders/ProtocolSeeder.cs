using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class ProtocolSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var protocol = new Protocol { Uri = "urn:oasis:names:tc:SAML:2.0:protocol" };
            context.Add<Protocol>(protocol);
            Seeder._cache.Add(Seeder.ProtocolsKey, new[] { protocol });
        }
    }
}