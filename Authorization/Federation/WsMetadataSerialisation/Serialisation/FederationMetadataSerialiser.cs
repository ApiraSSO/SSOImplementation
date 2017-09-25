using System.IdentityModel.Metadata;
using System.IO;
using System.Xml;
using Kernel.Federation.MetaData;

namespace WsMetadataSerialisation.Serialisation
{
    public class FederationMetadataSerialiser : MetadataSerializer, IMetadataSerialiser<MetadataBase>
    {
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
    }
}