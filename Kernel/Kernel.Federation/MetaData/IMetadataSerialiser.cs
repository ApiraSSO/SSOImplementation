using System.IO;
using System.Xml;
using Kernel.Cryptography.Validation;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataSerialiser<TMetadata>
    {
        ICertificateValidator Validator { get; }
        void Serialise(XmlWriter writer, TMetadata metadata);
        TMetadata Deserialise(Stream stream);
        TMetadata Deserialise(XmlReader xmlReader);
    }
}