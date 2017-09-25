using System.IO;
using System.Xml;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataSerialiser<TMetadata>
    {
        void Serialise(XmlWriter writer, TMetadata metadata);
        TMetadata Deserialise(Stream stream);
        TMetadata Deserialise(XmlReader xmlReader);
    }
}