using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class EntityDescriptorSeeder : Seeder
    {
        public override byte SeedingOrder { get { return 2; } }

        public override void Seed(IDbContext context)
        {
            var descriptor = new EntityDescriptorSettings
            {
                EntityId = "https://imperial.flowz_test.co.uk/",
                FederationId = String.Format("{0}_{1}", "flowz", Guid.NewGuid()),
                CacheDuration = new DatepartValue { Value = 100, Datepart = Datapart.Day },
                ValidUntil = DateTimeOffset.Now.AddDays(90),
            };

            //organisation
            if (Seeder._cache.ContainsKey(Seeder.Organisation))
            {
                var organisation = Seeder._cache[Seeder.Organisation] as OrganisationSettings;
                descriptor.Organisation = organisation;
            }
            //sp descriptors
            var spDescriptors = Seeder._cache[Seeder.SPDescriptorsKey] as IEnumerable<SPDescriptorSettings>;
            spDescriptors.Aggregate(descriptor, (d, next) => { d.RoleDescriptors.Add(next); return d; });

            context.Add<EntityDescriptorSettings>(descriptor);
            Seeder._cache[Seeder.EntityDescriptor] = descriptor;
        }
    }
}