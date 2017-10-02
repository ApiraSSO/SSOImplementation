using System.IdentityModel.Tokens;
using System.Xml;

namespace Kernel.Federation.Protocols.Response
{
    public interface ITokenHandler
    {
        Saml2Assertion GetAssertion(XmlReader reader, string parnerId);
        SecurityToken ReadToken(XmlReader reader, string parnerId);
    }
}