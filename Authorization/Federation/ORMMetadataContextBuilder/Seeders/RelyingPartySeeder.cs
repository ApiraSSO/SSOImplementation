using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;
using ORMMetadataContextProvider.Models.GlobalConfiguration;

namespace ORMMetadataContextProvider.Seeders
{
    internal class RelyingPartySeeder : Seeder
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

            var imperialRelyingParty = new RelyingPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://shibboleth.imperial.ac.uk/idp/shibboleth",
                MetadataLocation = "HTTP",
                RelyingPartyId = "imperial.ac.uk"
            };
            imperialRelyingParty.MetadataSettings = metadata;
            imperialRelyingParty.SecuritySettings = security;

            //shibboleth test metadata settings
            var testRelyingParty = new RelyingPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://www.testshib.org/metadata/testshib-providers.xml",
                MetadataLocation = "HTTP",
                RelyingPartyId = "testShib"
            };
            testRelyingParty.MetadataSettings = metadata;
            testRelyingParty.SecuritySettings = security;

            //local
            var localRelyingParty = new RelyingPartySettings
            {
                RefreshInterval = 30,
                AutoRefreshInterval = 1000,
                MetadataPath = "https://dg-mfb/idp/shibboleth",
                MetadataLocation = "HTTP",
                RelyingPartyId = "local"
            };
            localRelyingParty.MetadataSettings = metadata;
            localRelyingParty.SecuritySettings = security;

            context.Add<RelyingPartySettings>(imperialRelyingParty);
            context.Add<RelyingPartySettings>(testRelyingParty);
            context.Add<RelyingPartySettings>(localRelyingParty);

            metadata.RelyingParties.Add(imperialRelyingParty);
            metadata.RelyingParties.Add(localRelyingParty);
            metadata.RelyingParties.Add(testRelyingParty);

            security.RelyingParties.Add(imperialRelyingParty);
            security.RelyingParties.Add(localRelyingParty);
            security.RelyingParties.Add(testRelyingParty);
        }
    }
}