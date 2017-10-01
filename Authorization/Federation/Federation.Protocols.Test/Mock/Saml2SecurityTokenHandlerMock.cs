using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Federation.Protocols.Test.Mock
{
    public class Saml2SecurityTokenHandlerMock : Saml2SecurityTokenHandler
    {
        public Saml2Assertion GetAssertion(XmlReader reader)
        {
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
