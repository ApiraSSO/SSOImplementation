using System;
using System.IdentityModel.Metadata;
using System.Linq;
using Federation.Metadata.Consumer.Tests.Mock;
using Federation.Metadata.FederationPartner.Handlers;
using NUnit.Framework;
using Shared.Federtion.Constants;
using Shared.Federtion.Factories;

namespace Federation.Metadata.Consumer.Tests
{
    [TestFixture]
    internal class MetadataHandlerFactoryTests
    {
        [Test]
        public void GetDelegateForIdpDescriptors_entity_descriptor_metadata_Test()
        {
            //ARRANGE
            var metadata = EntityDescriptorProviderMock.GetEntityDescriptor();
            var handler = new MetadataEntitityDescriptorHandler();
            //ACT
            var del = IdpMetadataHandlerFactory.GetDelegateForIdpDescriptors(typeof(EntityDescriptor), typeof(IdentityProviderSingleSignOnDescriptor));
            var idps = del(handler, metadata)
                .ToList();
            //ASSERT
            Assert.AreEqual(1, idps.Count);
        }

        [Test]
        public void GetDelegateForIdpDescriptors_entities_descriptor_metadata_Test()
        {
            //ARRANGE
            var metadata = EntityDescriptorProviderMock.GetEntitiesDescriptor();
            var handler = new MetadataEntitiesDescriptorHandler();
            //ACT
            var del = IdpMetadataHandlerFactory.GetDelegateForIdpDescriptors(typeof(EntitiesDescriptor), typeof(IdentityProviderSingleSignOnDescriptor));
            var idps = del(handler, metadata)
                .ToList();
            //ASSERT
            Assert.AreEqual(1, idps.Count);
        }

        #region metadata Path tests

        [Test]
        public void GetDelegateForIdpLocation_entity_descriptor_metadata_Test()
        {
            //ARRANGE
            var expected = new Uri("http://localhost:60879");
            var metadata = EntityDescriptorProviderMock.GetEntityDescriptor();
            var handler = new MetadataEntitityDescriptorHandler();
            //ACT
            var del = IdpMetadataHandlerFactory.GetDelegateForIdpLocation(typeof(EntityDescriptor));
            var uri = del(handler, metadata, new Uri(ProtocolBindings.HttpRedirect));
            //ASSERT
            Assert.AreEqual(expected, uri);
        }

        [Test]
        public void GetDelegateForIdpLocation_entities_descriptor_metadata_Test()
        {
            //ARRANGE
            var expected = new Uri("http://localhost:60879");
            var metadata = EntityDescriptorProviderMock.GetEntitiesDescriptor();
            var handler = new MetadataEntitiesDescriptorHandler();
            //ACT
            var del = IdpMetadataHandlerFactory.GetDelegateForIdpLocation(typeof(EntitiesDescriptor));
            var uri = del(handler, metadata, new Uri(ProtocolBindings.HttpRedirect));
            //ASSERT
            Assert.AreEqual(expected, uri);
        }
        #endregion
    }
}