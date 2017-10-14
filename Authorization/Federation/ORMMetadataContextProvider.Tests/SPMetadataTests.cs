using System;
using System.IO;
using System.Linq;
using System.Xml;
using Kernel.Data;
using Kernel.Data.ORM;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.MetaData;
using Kernel.Reflection;
using NUnit.Framework;
using ORMMetadataContextProvider.Security;
using ORMMetadataContextProvider.Tests.Mock;
using Provider.EntityFramework;
using SecurityManagement;
using WsFederationMetadataProvider.Metadata;
using WsMetadataSerialisation.Serialisation;

namespace ORMMetadataContextProvider.Tests
{
    [TestFixture]
    public class SPMetadataTests
    {
        [Test]
        [Ignore("Create file")]
        public void SPMetadataGenerationTest_write_to_file()
        {
            ////ARRANGE

            var result = false;
            var path = @"D:\Dan\Software\Apira\SPMetadata\SPMetadataTest.xml";
            var metadataWriter = new TestMetadatWriter(el =>
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (var writer = XmlWriter.Create(path))
                {
                    el.WriteTo(writer);
                    writer.Flush();
                }
                result = true;
            });

            var cacheProvider = new CacheProviderMock();
            var customConfiguration = new DbCustomConfiguration();
            var connectionStringProvider = new MetadataConnectionStringProviderMock();
            var models = ReflectionHelper.GetAllTypes(new[] { typeof(MetadataContextBuilder).Assembly })
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(BaseModel).IsAssignableFrom(t));
            customConfiguration.ModelsFactory = () => models;

            var seeders = ReflectionHelper.GetAllTypes(new[] { typeof(MetadataContextBuilder).Assembly })
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ISeeder).IsAssignableFrom(t))
                .Select(x => (ISeeder)Activator.CreateInstance(x));
            seeders
                .OrderBy(x => x.SeedingOrder)
                .Aggregate(customConfiguration.Seeders, (c, next) => { c.Add(next); return c; });

            object dbcontext = new DBContext(connectionStringProvider, customConfiguration);

            var metadataContextBuilder = new MetadataContextBuilder((IDbContext)dbcontext, cacheProvider);
            var metadataRequest = new MetadataGenerateRequest(MetadataType.SP, "local");
            var metadatContext = metadataContextBuilder.BuildContext(metadataRequest);
            var context = new FederationPartyConfiguration(metadataRequest.FederationPartyId, "localhost") { MetadataContext = metadatContext };
            var logger = new LogProviderMock();

            var configurationProvider = new CertificateValidationConfigurationProvider((IDbContext)dbcontext, cacheProvider);
            var certificateValidator = new CertificateValidator(configurationProvider);
            var ssoCryptoProvider = new CertificateManager(logger);
            
            var metadataSerialiser = new FederationMetadataSerialiser(certificateValidator, logger);
            var metadataDispatcher = new FederationMetadataDispatcherMock(() => new[] { metadataWriter });
            
            var sPSSOMetadataProvider = new SPSSOMetadataProvider(metadataDispatcher, ssoCryptoProvider, metadataSerialiser, g => context, logger);
            
            //ACT
            sPSSOMetadataProvider.CreateMetadata(metadataRequest).Wait();
            //ASSERT
            Assert.IsTrue(result);
        }
    }
}