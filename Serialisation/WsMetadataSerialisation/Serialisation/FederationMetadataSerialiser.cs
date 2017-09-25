using System.IdentityModel.Metadata;
using System.IdentityModel.Selectors;
using System.IO;
using System.Xml;
using Kernel.Cryptography.Validation;
using Kernel.Federation.MetaData;

namespace WsMetadataSerialisation.Serialisation
{
    public class FederationMetadataSerialiser : MetadataSerializer, IMetadataSerialiser<MetadataBase>
    {
        public FederationMetadataSerialiser(ICertificateValidator certificateValidator)
        {
            base.CertificateValidator = (X509CertificateValidator)certificateValidator;
            base.CertificateValidationMode = certificateValidator.X509CertificateValidationMode;
        }
        public void Serialise(XmlWriter writer, MetadataBase metadata)
        {
            base.WriteMetadata(writer, metadata);
        }

        public MetadataBase Deserialise(Stream stream)
        {
            return base.ReadMetadata(stream);
        }

        public MetadataBase Deserialise(XmlReader xmlReader)
        {
            return base.ReadMetadata(xmlReader);
        }

        protected override bool ReadCustomElement<T>(XmlReader reader, T target)
        {
            return base.ReadCustomElement(reader, target);
        }
    }
}