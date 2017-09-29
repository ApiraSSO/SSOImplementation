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
        private readonly ICertificateValidator _certificateValidator;
        public FederationMetadataSerialiser(ICertificateValidator certificateValidator)
        {
            this._certificateValidator = certificateValidator;
            base.CertificateValidator = (X509CertificateValidator)certificateValidator;
        }
        public ICertificateValidator Validator { get { return base.CertificateValidator as ICertificateValidator; } }
        public void Serialise(XmlWriter writer, MetadataBase metadata)
        {
            base.WriteMetadata(writer, metadata);
        }

        public MetadataBase Deserialise(Stream stream)
        {
            base.CertificateValidationMode = this._certificateValidator.X509CertificateValidationMode;
            return base.ReadMetadata(stream);
        }

        public MetadataBase Deserialise(XmlReader xmlReader)
        {
            base.CertificateValidationMode = this._certificateValidator.X509CertificateValidationMode;
            return base.ReadMetadata(xmlReader);
        }

        protected override bool ReadCustomElement<T>(XmlReader reader, T target)
        {
            return base.ReadCustomElement(reader, target);
        }
    }
}