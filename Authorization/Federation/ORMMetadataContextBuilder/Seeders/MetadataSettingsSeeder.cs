using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class MetadataSettingsSeeder : Seeder
    {
        public override byte SeedingOrder
        {
            get { return 10; }
        }
        public override void Seed(IDbContext context)
        {
            var entityDescriptor = Seeder._cache[Seeder.EntityDescriptor] as EntityDescriptorSettings;
            var signing = Seeder._cache[Seeder.Signing] as SigningCredential;
            var metadataSettings = new MetadataSettings
            {
                SigningCredential = signing,
                SPDescriptorSettings = entityDescriptor
            };
            context.Add<MetadataSettings>(metadataSettings);
            Seeder._cache[Seeder.Metadata] = metadataSettings;
        }
    }
}