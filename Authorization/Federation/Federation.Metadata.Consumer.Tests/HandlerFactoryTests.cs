using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Federation.Metadata.Consumer.Tests.Mock;
using Federation.Metadata.FederationPartner.Handlers;
using NUnit.Framework;

namespace Federation.Metadata.Consumer.Tests
{
    [TestFixture]
    internal class HandlerFactoryTests
    {
        [Test]
        public void GetDelegateForIdpDescriptors_entity_descriptor_metadata_Test()
        {
            //ARRANGE
            var metadata = EntityDescriptorProviderMock.GetEntityDescriptor();
            var handler = new MetadataEntitityDescriptorHandler();
            //ACT
            var del = HandlerFactory.GetDelegateForIdpDescriptors(typeof(EntityDescriptor), typeof(IdentityProviderSingleSignOnDescriptor));
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
            var del = HandlerFactory.GetDelegateForIdpDescriptors(typeof(EntitiesDescriptor), typeof(IdentityProviderSingleSignOnDescriptor));
            var idps = del(handler, metadata)
                .ToList();
            //ASSERT
            Assert.AreEqual(1, idps.Count);
        }
    }
}
