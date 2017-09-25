using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Kernel.Data.ORM;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class CertificateSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var certificate = new Certificate
            {
                IsDefault = true,
                Use = Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Signing,
                CetrificateStore = "TestCertStore",
                StoreLocation = StoreLocation.LocalMachine
            };
            var storeCriterion = new StoreSearchCriterion
            {
                SearchCriteriaType = System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName,
                SearchCriteria = "ApiraTestCertificate",
            };
            var signingCritencials = new SigningCredential
            {
                DigestAlgorithm = SecurityAlgorithms.Sha1Digest,
                SignatureAlgorithm = SecurityAlgorithms.RsaSha1Signature,
            };
            certificate.SigningCredentials.Add(signingCritencials);
            signingCritencials.Certificates.Add(certificate);
            certificate.StoreSearchCriteria.Add(storeCriterion);
            context.Add<Certificate>(certificate);
            Seeder._cache.Add(Seeder.CertificatesKey, new[] { certificate });
        }
    }
}