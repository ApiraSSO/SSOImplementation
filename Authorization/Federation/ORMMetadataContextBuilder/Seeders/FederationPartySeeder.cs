using System.Collections.Generic;
using System.Linq;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;
using ORMMetadataContextProvider.Models.GlobalConfiguration;
using Shared.Federtion.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class FederationPartySeeder : Seeder
    {
        public override byte SeedingOrder
        {
            get
            {
                return 100;
            }
        }

        //imperial collage settings
        public override void Seed(IDbContext context)
        {
            var metadata = Seeder._cache[Seeder.Metadata] as MetadataSettings;
            var security = Seeder._cache[Seeder.Security] as SecuritySettings;
            
            var authnRequestSettings = this.GetAutnRequestSettings();

            var imperialFederationParty = new FederationPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://shibboleth.imperial.ac.uk/idp/shibboleth",
                MetadataLocation = "HTTP",
                FederationPartyId = "imperial.ac.uk"
            };
            imperialFederationParty.MetadataSettings = metadata;
            imperialFederationParty.SecuritySettings = security;
            imperialFederationParty.AutnRequestSettings = authnRequestSettings;

            //shibboleth test metadata settings
            var testFederationParty = new FederationPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://www.testshib.org/metadata/testshib-providers.xml",
                MetadataLocation = "HTTP",
                FederationPartyId = "testShib"
            };
            testFederationParty.MetadataSettings = metadata;
            testFederationParty.SecuritySettings = security;
            testFederationParty.AutnRequestSettings = authnRequestSettings;

            //local
            var localFederationParty = new FederationPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://dg-mfb/idp/shibboleth",
                MetadataLocation = "HTTP",
                FederationPartyId = "local"
            };
            localFederationParty.MetadataSettings = metadata;
            localFederationParty.SecuritySettings = security;
            localFederationParty.AutnRequestSettings = authnRequestSettings;

            context.Add<FederationPartySettings>(imperialFederationParty);
            context.Add<FederationPartySettings>(testFederationParty);
            context.Add<FederationPartySettings>(localFederationParty);

            metadata.RelyingParties.Add(imperialFederationParty);
            metadata.RelyingParties.Add(localFederationParty);
            metadata.RelyingParties.Add(testFederationParty);

            security.RelyingParties.Add(imperialFederationParty);
            security.RelyingParties.Add(localFederationParty);
            security.RelyingParties.Add(testFederationParty);
        }

        private AutnRequestSettings GetAutnRequestSettings()
        {
            var authnContexts = Seeder._cache[Seeder.SamlAutnContextKey] as IEnumerable<SamlAutnContext>;
            var nameIds = Seeder._cache[Seeder.NameIdKey] as IEnumerable<NameIdFormat>;

            var persistentNameId = nameIds
                .First(x => x.Key == "Persistent");
            var transientNameId = nameIds
                .First(x => x.Key == "Transient");

            var settings = new AutnRequestSettings
            {
                ForceAuthn = false,
                IsPassive = false,
                Version = "2.0",
                RequitedAutnContext = new RequitedAutnContext
                {
                    Comparison = AuthnContextComparisonType.Exact
                },
                NameIdConfiguration = new NameIdConfiguration
                {
                    AllowCreate = false,
                    EncryptNameId = false,
                    DefaultNameIdFormat = transientNameId
                }
            };

            authnContexts.Aggregate(settings.RequitedAutnContext.RequitedAuthnContexts, (t, next) => 
            {
                t.Add(next);
                return t;
            });

            return settings;
        }
    }
}