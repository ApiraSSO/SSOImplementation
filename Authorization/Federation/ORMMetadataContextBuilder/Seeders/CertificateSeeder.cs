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
            var signingCertificate = new Certificate
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
            var encryptionCertificate = new Certificate
            {
                IsDefault = true,
                Use = Kernel.Federation.MetaData.Configuration.Cryptography.KeyUsage.Encryption,
                CetrificateStore = "TestCertStore",
                StoreLocation = StoreLocation.LocalMachine
            };
            var encryptionStoreCriterion = new StoreSearchCriterion
            {
                SearchCriteriaType = System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName,
                SearchCriteria = "Apira_DevEnc",
            };
            encryptionCertificate.StoreSearchCriteria.Add(encryptionStoreCriterion);

            var signingCritencials = new SigningCredential
            {
                DigestAlgorithm = SecurityAlgorithms.Sha1Digest,
                SignatureAlgorithm = SecurityAlgorithms.RsaSha1Signature,
            };
            signingCertificate.SigningCredentials.Add(signingCritencials);
            signingCritencials.Certificates.Add(signingCertificate);
            signingCertificate.StoreSearchCriteria.Add(storeCriterion);
            context.Add<Certificate>(signingCertificate);
            Seeder._cache.Add(Seeder.CertificatesKey, new[] { signingCertificate, encryptionCertificate });
            Seeder._cache.Add(Seeder.Signing, signingCritencials);
        }
    }
}