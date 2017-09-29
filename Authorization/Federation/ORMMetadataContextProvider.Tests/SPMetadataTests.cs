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
            var context = new FederationPartyContext(metadataRequest.FederationPartyId, "localhost");
           

            var configurationProvider = new CertificateValidationConfigurationProvider((IDbContext)dbcontext, cacheProvider);
            var certificateValidator = new CertificateValidator(configurationProvider);
            var ssoCryptoProvider = new CertificateManager();
            
            var metadataSerialiser = new FederationMetadataSerialiser(certificateValidator);
           
            var sPSSOMetadataProvider = new SPSSOMetadataProvider(metadataWriter, ssoCryptoProvider, metadataSerialiser, g => context);
            
            //ACT
            sPSSOMetadataProvider.CreateMetadata(metadataRequest);
            //ASSERT
            Assert.IsTrue(result);
        }

        //[Test]
        //[Ignore("Create file")]
        //public void SPMetadataGeneration_create_file()
        //{
        //    ////ARRANGE

        //    var result = false;
        //    var path = @"D:\Dan\Software\Apira\SPMetadata\SPMetadataTest.xml";
        //    var metadataWriter = new TestMetadatWriter(el =>
        //    {
        //        if (File.Exists(path))
        //            File.Delete(path);

        //        using (var writer = XmlWriter.Create(path))
        //        {
        //            el.WriteTo(writer);
        //            writer.Flush();
        //        }
        //        result = true;
        //    });


        //    var contextBuilder = new InlineMetadataContextBuilder();
        //    var metadataRequest = new MetadataGenerateRequest(MetadataType.SP, "local");
        //    var context = contextBuilder.BuildContext(metadataRequest);

        //    var configurationProvider = new CertificateValidationConfigurationProvider();
        //    var certificateValidator = new CertificateValidator(configurationProvider);
        //    var ssoCryptoProvider = new CertificateManager();

        //    var metadataSerialiser = new FederationMetadataSerialiser(certificateValidator);

        //    var sPSSOMetadataProvider = new SPSSOMetadataProvider(metadataWriter, ssoCryptoProvider, metadataSerialiser, g => context);

        //    //ACT
        //    sPSSOMetadataProvider.CreateMetadata(metadataRequest);
        //    //ASSERT
        //    Assert.IsTrue(result);
        //}

        //[Test]
        //public void SPMetadata_serialise_deserialise_Test()
        //{
        //    ////ARRANGE

        //    string metadataXml = String.Empty;
        //    var metadataWriter = new TestMetadatWriter(el => metadataXml = el.OuterXml);
            
        //    var contextBuilder = new InlineMetadataContextBuilder();
        //    var metadataRequest = new MetadataGenerateRequest(MetadataType.SP, "local");
        //    var context = contextBuilder.BuildContext(metadataRequest);

        //    var configurationProvider = new CertificateValidationConfigurationProvider();
        //    var certificateValidator = new CertificateValidator(configurationProvider);
        //    var ssoCryptoProvider = new CertificateManager();

        //    var metadataSerialiser = new FederationMetadataSerialiser(certificateValidator);

        //    var sPSSOMetadataProvider = new SPSSOMetadataProvider(metadataWriter, ssoCryptoProvider, metadataSerialiser, g => context);
            
        //    //ACT
        //    sPSSOMetadataProvider.CreateMetadata(metadataRequest);
        //    var xmlReader = XmlReader.Create(new StringReader(metadataXml));
        //    var deserialisedMetadata = metadataSerialiser.ReadMetadata(xmlReader) as EntityDescriptor;
        //    //ASSERT
        //    Assert.IsFalse(String.IsNullOrWhiteSpace(metadataXml));
        //    Assert.AreEqual(1, deserialisedMetadata.RoleDescriptors.Count);
        //}
    }
}