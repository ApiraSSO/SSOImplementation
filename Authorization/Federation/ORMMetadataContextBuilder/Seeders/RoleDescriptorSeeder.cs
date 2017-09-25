using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class RoleDescriptorSeeder : Seeder
    {
        public override byte SeedingOrder { get { return 1; } }

        public override void Seed(IDbContext context)
        {
            var roleDescriptor = new RoleDescriptorSettings
            {
                CacheDuration = 100,
                ValidUntil = DateTimeOffset.Now.AddDays(90),
                ErrorUrl = "http://localhost:60879/api/Account/Error"
            };
            var protocols = Seeder._cache[Seeder.ProtocolsKey] as IEnumerable<Protocol>;
            protocols.Aggregate(roleDescriptor, (d, next) => 
            {
                next.RoleDescriptors.Add(roleDescriptor);
                d.Protocols.Add(next);
                return d;
            });

            var certificates = Seeder._cache[Seeder.CertificatesKey] as IEnumerable<Certificate>;
            certificates.Aggregate(roleDescriptor, (d, next) => 
            {
                d.Certificates.Add(next);
                next.RoleDescriptors.Add(roleDescriptor);
                return d;
            });

            context.Add<RoleDescriptorSettings>(roleDescriptor);
        }
    }
}