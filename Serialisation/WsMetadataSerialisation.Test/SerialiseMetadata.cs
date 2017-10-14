using System;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Xml;
using InlineMetadataContextProvider;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using NUnit.Framework;
using SecurityManagement;
using WsFederationMetadataProvider.Metadata.DescriptorBuilders;
using WsMetadataSerialisation.Serialisation;
using WsMetadataSerialisation.Test.Mock;

namespace WsMetadataSerialisation.Test
{
    [TestFixture]
    public class SerialiseMetadata
    {
        [Test]
        public void SerialiseMetadataTest()
        {
            //ARRANGE
            var contextBuilder = new InlineMetadataContextBuilder();
            var metadataRequest = new MetadataGenerateRequest(MetadataType.SP, "local");
            var context = contextBuilder.BuildContext(metadataRequest);

            var configurationProvider = new CertificateValidationConfigurationProvider();
            var certificateValidator = new CertificateValidator(configurationProvider);
            var metadata = context.EntityDesriptorConfiguration;
            var spDescriptorConfigurtion = context.EntityDesriptorConfiguration.RoleDescriptors.First() as SPSSODescriptorConfiguration;
            var descriptorBuilder = new ServiceProviderSingleSignOnDescriptorBuilder();
            
            var descriptor = descriptorBuilder.BuildDescriptor(spDescriptorConfigurtion);
            var entityDescriptor = new EntityDescriptor(new EntityId("EntityIdTest"));
            entityDescriptor.RoleDescriptors.Add(descriptor);
            var logger = new LogProviderMock();
            var metadataSerialiser = new FederationMetadataSerialiser(certificateValidator, logger);
            //ACT
            var sb = new StringBuilder();

            using (var xmlWriter = XmlWriter.Create(sb))
            {
                metadataSerialiser.Serialise(xmlWriter, entityDescriptor);
            }
            var xmlResult = sb.ToString();

            //ASSERT
            Assert.IsFalse(String.IsNullOrWhiteSpace(xmlResult));
        }
    }
}