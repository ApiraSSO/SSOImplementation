using System.IdentityModel.Tokens;
using System.Xml;
using Federation.Protocols.Tokens;

namespace Federation.Protocols.Test.Mock.Tokens
{
    internal class SecurityTokenHandlerMock : Saml2SecurityTokenHandler
    {
        public void SetConfiguration(SecurityTokenHandlerConfiguration configuration)
        {
            base.Configuration = configuration;
        }
        public Saml2Assertion GetAssertion(XmlReader reader)
        {
            TokenHelper.MoveToToken(reader);
            return base.ReadAssertion(reader);
        }
    }
}