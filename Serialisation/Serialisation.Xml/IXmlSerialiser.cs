using System.Xml;
using Kernel.Serialisation;

namespace Serialisation.Xml
{
    public interface IXmlSerialiser : ISerializer
    {
        void Serialise<T>(XmlWriter xmlWriter, T value);
        T Deserialise<T>(XmlReader reader);
    }
}
