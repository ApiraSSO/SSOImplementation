using System;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;

namespace WsFederationMetadataProvider.Extensions
{
    public static class EntityDescriptorExtensions
    {
        public static XmlElement ToXml(this EntityDescriptor descriptor)
        {
            var xmlDocument = new XmlDocument()
            {
                PreserveWhitespace = true,
                XmlResolver = (XmlResolver)null
            };

            XmlElement element = xmlDocument.CreateElement("md", Saml2MetadataConstants.Elements.EntitiesDescriptor, "urn:oasis:names:tc:SAML:2.0:metadata");
            
            if (descriptor.EntityId != null)
                element.SetAttribute(Saml2MetadataConstants.Attributes.EntityId, descriptor.EntityId.ToString());
            //if (this.validUntil != DateTime.MaxValue)
            //    element.SetAttribute("validUntil", SAML.ToDateTimeString(this.validUntil));
            //if (this.cacheDuration != null)
            //    element.SetAttribute("cacheDuration", this.cacheDuration.ToString());
            if (!string.IsNullOrEmpty(descriptor.FederationId))
                element.SetAttribute(Saml2MetadataConstants.Attributes.Id, descriptor.FederationId);
            foreach(var role in descriptor.RoleDescriptors)
            {
                element.AppendChild((XmlNode)EntityDescriptorExtensions.ToXml(role, xmlDocument));
            }
            //if (this.signature != null)
            //    element.AppendChild(xmlDocument.ImportNode((XmlNode)this.signature, true));
            //if (this.extensions != null)
            //    element.AppendChild((XmlNode)this.extensions.ToXml(xmlDocument));
            //foreach (RoleDescriptor roleDescriptor in (IEnumerable<RoleDescriptor>)this.roleDescriptors)
            //    element.AppendChild((XmlNode)roleDescriptor.ToXml(xmlDocument));
            //foreach (IDPSSODescriptor idpSsoDescriptor in (IEnumerable<IDPSSODescriptor>)this.idpSSODescriptors)
            //    element.AppendChild((XmlNode)idpSsoDescriptor.ToXml(xmlDocument));
            //foreach (SPSSODescriptor spSsoDescriptor in (IEnumerable<SPSSODescriptor>)this.spSSODescriptors)
            //    element.AppendChild((XmlNode)spSsoDescriptor.ToXml(xmlDocument));
            //foreach (AuthnAuthorityDescriptor authorityDescriptor in (IEnumerable<AuthnAuthorityDescriptor>)this.authnAuthorityDescriptors)
            //    element.AppendChild((XmlNode)authorityDescriptor.ToXml(xmlDocument));
            //foreach (AttributeAuthorityDescriptor authorityDescriptor in (IEnumerable<AttributeAuthorityDescriptor>)this.attributeAuthorityDescriptors)
            //    element.AppendChild((XmlNode)authorityDescriptor.ToXml(xmlDocument));
            //foreach (PDPDescriptor pdpDescriptor in (IEnumerable<PDPDescriptor>)this.pdpDescriptors)
            //    element.AppendChild((XmlNode)pdpDescriptor.ToXml(xmlDocument));
            //if (this.affiliationDescriptor != null)
            //    element.AppendChild((XmlNode)this.affiliationDescriptor.ToXml(xmlDocument));
            //if (this.organization != null)
            //    element.AppendChild((XmlNode)this.organization.ToXml(xmlDocument));
            //foreach (ContactPerson contactPerson in (IEnumerable<ContactPerson>)this.contactPeople)
            //    element.AppendChild((XmlNode)contactPerson.ToXml(xmlDocument));
            //foreach (AdditionalMetadataLocation metadataLocation in (IEnumerable<AdditionalMetadataLocation>)this.additionalMetadataLocations)
            //    element.AppendChild((XmlNode)metadataLocation.ToXml(xmlDocument));
            xmlDocument.AppendChild((XmlNode)element);
            return element;
        }

        public static XmlElement ToXml(this RoleDescriptor descriptor, XmlDocument xmlDocument)
        {
            //ServiceProviderSingleSignOnDescriptor
            XmlElement element1 = xmlDocument.CreateElement("md", Saml2MetadataConstants.Elements.SpssoDescriptor, "urn:oasis:names:tc:SAML:2.0:metadata");
            //this.ToXml(element1);
            //element1.SetAttribute("AuthnRequestsSigned", this.authnRequestsSigned ? "true" : "false");
            //element1.SetAttribute("WantAssertionsSigned", this.wantAssertionsSigned ? "true" : "false");
            //foreach (IndexedEndpointType assertionConsumerService in (IEnumerable<IndexedEndpointType>)this.assertionConsumerServices)
            //{
            //    XmlElement element2 = element1.OwnerDocument.CreateElement("md", "AssertionConsumerService", "urn:oasis:names:tc:SAML:2.0:metadata");
            //    assertionConsumerService.ToXml(element2);
            //    element1.AppendChild((XmlNode)element2);
            //}
            //foreach (AttributeConsumingService consumingService in (IEnumerable<AttributeConsumingService>)this.attributeConsumingServices)
            //    element1.AppendChild((XmlNode)consumingService.ToXml(xmlDocument));
            return element1;
        }
    }
}