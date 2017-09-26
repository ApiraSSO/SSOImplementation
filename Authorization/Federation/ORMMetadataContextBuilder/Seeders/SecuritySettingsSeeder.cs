using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models.GlobalConfiguration;

namespace ORMMetadataContextProvider.Seeders
{
    internal class SecuritySettingsSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var securitySettings = new SecuritySettings
            {
                X509CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom,
                PinnedValidation = false
            };
            context.Add<SecuritySettings>(securitySettings);
            Seeder._cache[Seeder.Security] = securitySettings;
            
        }
    }
}