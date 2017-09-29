using System.IO;
using System.Xml;

namespace Kernel.Federation.MetaData
{
    public interface IFederationMetadataWriter
    {
        void Write(XmlElement xml, Stream target);
    }
}