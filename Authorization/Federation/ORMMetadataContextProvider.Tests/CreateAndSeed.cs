using System;
using System.Linq;
using Kernel.Data;
using Kernel.Data.ORM;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using Kernel.Reflection;
using NUnit.Framework;
using ORMMetadataContextProvider.Tests.Mock;
using Provider.EntityFramework;
using WsFederationMetadataProvider.Metadata.DescriptorBuilders;

namespace ORMMetadataContextProvider.Tests
{
    [TestFixture]
    public class CreateAndSeed
    {
        [Test]
        public void Test1()
        {
            //ARRANGE
            var cacheProvider = new CacheProviderMock();
            var customConfiguration = new DbCustomConfiguration();
            var connectionStringProvider = new MetadataConnectionStringProviderMock();
            var models = ReflectionHelper.GetAllTypes(new[] {typeof(MetadataContextBuilder).Assembly })
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(BaseModel).IsAssignableFrom(t));
            customConfiguration.ModelsFactory = () => models;

            var seeders = ReflectionHelper.GetAllTypes(new[] { typeof(MetadataContextBuilder).Assembly })
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ISeeder).IsAssignableFrom(t))
                .Select(x => (ISeeder)Activator.CreateInstance(x));
            seeders
                .OrderBy(x => x.SeedingOrder)
                .Aggregate(customConfiguration.Seeders, (c, next) => { c.Add(next); return c; });

            object context = new DBContext(connectionStringProvider, customConfiguration);
            
            var metadataContextBuilder = new MetadataContextBuilder((IDbContext)context, cacheProvider);
            var metadataRequest = new MetadataGenerateRequest(MetadataType.SP, "local");
            //ACT
            var metadata = metadataContextBuilder.BuildContext(metadataRequest);
            //ASSERT
        }

        [Test]
        public void ServiceProviderSingleSignOnDescriptorBuilderTest_db_contex_provider()
        {
            //ARRANGE
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
            var context = metadataContextBuilder.BuildContext(metadataRequest);
            var spDescriptorConfigurtion = context.EntityDesriptorConfiguration.RoleDescriptors.First() as SPSSODescriptorConfiguration;
            var descriptorBuilder = new ServiceProviderSingleSignOnDescriptorBuilder();
            //ACT
            var descriptor = descriptorBuilder.BuildDescriptor(spDescriptorConfigurtion);
            var organisation = descriptor.Organization;
            var protocolsSupported = descriptor.ProtocolsSupported;
            var assertionServices = descriptor.AssertionConsumerServices;
            var keys = descriptor.Keys;

            //ASSERT
            //assert sp descriptor attributes

            Assert.AreEqual(spDescriptorConfigurtion.WantAssertionsSigned, descriptor.WantAssertionsSigned);
            Assert.AreEqual(spDescriptorConfigurtion.AuthenticationRequestsSigned, descriptor.AuthenticationRequestsSigned);
            Assert.AreEqual(spDescriptorConfigurtion.AssertionConsumerServices.Count, descriptor.AssertionConsumerServices.Count);
            foreach (var s in spDescriptorConfigurtion.AssertionConsumerServices)
            {
                var descriptorService = assertionServices[s.Index];
                Assert.AreEqual(s.Index, descriptorService.Index);
                Assert.AreEqual(s.Location, descriptorService.Location);
                Assert.AreEqual(s.Binding, descriptorService.Binding);
                Assert.AreEqual(s.IsDefault, descriptorService.IsDefault);
            }

            //assert sso descriptor attributes
            Assert.AreEqual(spDescriptorConfigurtion.ArtifactResolutionServices.Count, descriptor.ArtifactResolutionServices.Count);
            foreach (var s in spDescriptorConfigurtion.ArtifactResolutionServices)
            {
                var descriptorService = descriptor.ArtifactResolutionServices[s.Index];
                Assert.AreEqual(s.Index, descriptorService.Index);
                Assert.AreEqual(s.Location, descriptorService.Location);
                Assert.AreEqual(s.Binding, descriptorService.Binding);
            }
            Assert.True(Enumerable.SequenceEqual(descriptor.NameIdentifierFormats, spDescriptorConfigurtion.NameIdentifierFormats));

            Assert.AreEqual(spDescriptorConfigurtion.SingleLogoutServices.Count, descriptor.SingleLogoutServices.Count);
            foreach (var s in spDescriptorConfigurtion.SingleLogoutServices)
            {
                var descriptorService = descriptor.SingleLogoutServices.Single(x => x.Location == s.Location);
                Assert.AreEqual(s.ResponseLocation, descriptorService.ResponseLocation);
                Assert.AreEqual(s.Binding, descriptorService.Binding);
            }

            //assert role descriptor attributes
            Assert.AreEqual(spDescriptorConfigurtion.ErrorUrl, descriptor.ErrorUrl);
            Assert.AreEqual(spDescriptorConfigurtion.ValidUntil.DateTime, descriptor.ValidUntil);
            Assert.True(Enumerable.SequenceEqual(descriptor.ProtocolsSupported, spDescriptorConfigurtion.ProtocolSupported));
            Assert.AreEqual(spDescriptorConfigurtion.KeyDescriptors.Count, descriptor.Keys.Count);
            for (var i = 0; i < spDescriptorConfigurtion.KeyDescriptors.Count; i++)
            {
                var descriptorKey = descriptor.Keys.ElementAt(i);
                var configKey = spDescriptorConfigurtion.KeyDescriptors.ElementAt(i);
                Assert.AreEqual(configKey.Use.ToString(), descriptorKey.Use.ToString());
            }

            //organisation
            Assert.AreEqual(spDescriptorConfigurtion.Organisation.Names.Count, organisation.Names.Count);
            foreach (var n in spDescriptorConfigurtion.Organisation.Names)
            {
                var targetName = organisation.Names[n.Language];
                Assert.AreEqual(n.Name, targetName.Name);
            }
            Assert.AreEqual(spDescriptorConfigurtion.Organisation.Names.Count, organisation.DisplayNames.Count);
            foreach (var n in spDescriptorConfigurtion.Organisation.Names)
            {
                var targetName = organisation.DisplayNames[n.Language];
                Assert.AreEqual(n.DisplayName, targetName.Name);
            }
            Assert.AreEqual(spDescriptorConfigurtion.Organisation.Urls.Count, organisation.Urls.Count);
            foreach (var n in spDescriptorConfigurtion.Organisation.Urls)
            {
                var targetName = organisation.Urls[n.Language];
                Assert.AreEqual(n.Url, targetName.Uri);
            }

            //contacts
            var configContacts = spDescriptorConfigurtion.Organisation.OrganisationContacts;
            Assert.AreEqual(configContacts.PersonContact.Count, descriptor.Contacts.Count);
            for (var i = 0; i < configContacts.PersonContact.Count; i++)
            {
                var source = configContacts.PersonContact.ElementAt(i);
                var targer = descriptor.Contacts.ElementAt(i);
                Assert.AreEqual(source.ContactType.ToString(), targer.Type.ToString());
                Assert.AreEqual(source.ForeName, targer.GivenName);
                Assert.AreEqual(source.SurName, targer.Surname);
                Assert.IsTrue(Enumerable.SequenceEqual(source.Emails, targer.EmailAddresses));
                Assert.IsTrue(Enumerable.SequenceEqual(source.PhoneNumbers, targer.TelephoneNumbers));
            }
        }
    }
}