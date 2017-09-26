using System.Xml;
using System.Xml.Serialization;
using Kernel.Serialisation;

namespace Serialisation.Xml
{
    public interface IXmlSerialiser : ISerializer
    {
        XmlSerializerNamespaces XmlNamespaces { get; }
        void Serialise<T>(XmlWriter xmlWriter, T value);
        T Deserialise<T>(XmlReader reader);
    }
}
