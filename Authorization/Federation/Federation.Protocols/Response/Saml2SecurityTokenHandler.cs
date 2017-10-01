using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Federation.Protocols.Request;
using Federation.Protocols.Request.Elements;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    public class Saml2SecurityTokenHandler : System.IdentityModel.Tokens.Saml2SecurityTokenHandler, ITokenHandler
    {
        //private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;

        public Saml2SecurityTokenHandler(ITokenHandlerConfigurationProvider tokenHandlerConfigurationProvider)
        {
            tokenHandlerConfigurationProvider.Configuration(this);
        }
        public Saml2Assertion GetAssertion(XmlReader reader)
        {
            while (!reader.IsStartElement(EncryptedAssertion.ElementName, Saml20Constants.Assertion))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find assertion element.");
            }
            
            return this.ReadAssertion(reader);
        }
        protected override Saml2Assertion ReadAssertion(XmlReader reader)
        {
            return base.ReadAssertion(reader);
        }
        protected override Saml2SubjectConfirmationData ReadSubjectConfirmationData(XmlReader reader)
        {
            var result = new Saml2SubjectConfirmationData();
            if (!reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion"))
                reader.ReadStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion");
            string attribute2 = reader.GetAttribute("InResponseTo");
            result.InResponseTo = new Saml2Id("test");
            reader.Read();
            reader.ReadEndElement();
            return result;
            return base.ReadSubjectConfirmationData(reader);
        }
    }
}
