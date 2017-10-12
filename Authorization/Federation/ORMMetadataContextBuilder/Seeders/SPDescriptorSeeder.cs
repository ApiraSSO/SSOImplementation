using System;
using System.Collections.Generic;
using System.Linq;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class SPDescriptorSeeder : Seeder
    {
        public override byte SeedingOrder { get { return 1; } }

        public override void Seed(IDbContext context)
        {
            var descriptor = new SPDescriptorSettings
            {
                WantAssertionsSigned = true,
                RequestSigned = true,
                CacheDuration = new DatepartValue { Value = 100, Datepart = Datapart.Day },
                ValidUntil = DateTimeOffset.Now.AddDays(90),
                ErrorUrl = "http://localhost:60879/api/Account/Error"
            };

            //nameids
            var nameIds = Seeder._cache[Seeder.NameIdKey] as IEnumerable<NameIdFormat>;
            nameIds
                .Where(x => x.Key == "Transient" || x.Key == "Persistent")
                .Aggregate(descriptor, (d, next) =>
                {
                    next.SSODescriptorSettings.Add(descriptor);
                    d.NameIdFormats.Add(next);
                    return d;
                });

            //role descriptor protocols
            var protocols = Seeder._cache[Seeder.ProtocolsKey] as IEnumerable<Protocol>;
            protocols.Aggregate(descriptor, (d, next) => 
            {
                next.RoleDescriptors.Add(descriptor);
                d.Protocols.Add(next);
                return d;
            });

            //role descriptor certificates
            var certificates = Seeder._cache[Seeder.CertificatesKey] as IEnumerable<Certificate>;
            certificates.Aggregate(descriptor, (d, next) => 
            {
                d.Certificates.Add(next);
                next.RoleDescriptors.Add(descriptor);
                return d;
            });

            var bindings = Seeder._cache[Seeder.BindingsKey] as IEnumerable<Binding>;
            var httpPostBinding = bindings.First(x => x.Name.Equals("HTTP-POST", StringComparison.OrdinalIgnoreCase));

            descriptor.LogoutServices.Add(new EndPointSetting { Binding = httpPostBinding, Url = "http://localhost:60879/api/Account/SSOLogout" });
            
            //sp descriptor assertion services
            
            descriptor.AssertionServices.Add(new IndexedEndPointSetting
            {
                Index = 0,
                IsDefault = true,
                Binding = httpPostBinding,
                Url = "http://localhost:60879/api/Account/SSOLogon"
            });

            context.Add<SPDescriptorSettings>(descriptor);
            Seeder._cache.Add(Seeder.SPDescriptorsKey, new[] { descriptor });
        }
    }
}