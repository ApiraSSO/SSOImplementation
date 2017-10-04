using System.IdentityModel.Tokens;
using System.Xml;
using Kernel.Serialisation;

namespace Kernel.Federation.Tokens
{
    public interface ITokenSerialiser : ISerializer
    {
        SecurityToken DeserialiseToken(XmlReader reader, string partnerId);
    }
}