using System.IdentityModel.Metadata;
using System.IdentityModel.Selectors;
using System.IO;
using System.Xml;
using Kernel.Cryptography.Validation;
using Kernel.Federation.MetaData;
using Kernel.Logging;

namespace WsMetadataSerialisation.Serialisation
{
    public class FederationMetadataSerialiser : MetadataSerializer, IMetadataSerialiser<MetadataBase>
    {
        private readonly ICertificateValidator _certificateValidator;
        private readonly ILogProvider _logProvider;
        public FederationMetadataSerialiser(ICertificateValidator certificateValidator, ILogProvider logProvider)
        {
            this._certificateValidator = certificateValidator;
            base.CertificateValidator = (X509CertificateValidator)certificateValidator;
            this._logProvider = logProvider;
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