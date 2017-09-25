using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace ComponentSpace.SAML2.Metadata.Provider.Signing
{
    internal class SignedMetadata : SignedXml
    {
        public SignedMetadata()
        {
        }

        public SignedMetadata(XmlDocument xmlDocument)
          : base(xmlDocument)
        {
        }

        public SignedMetadata(XmlElement xmlElement)
          : base(xmlElement)
        {
        }

        public override XmlElement GetIdElement(XmlDocument document, string id)
        {
            if (document == null)
                return (XmlElement)null;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("//*[namespace-uri(.) = '{0}' and @{1}='{2}']", (object)"urn:oasis:names:tc:SAML:2.0:metadata", (object)"ID", (object)id);
            string xpath = stringBuilder.ToString();
            XmlElement xmlElement = (XmlElement)document.SelectSingleNode(xpath);
            
            return xmlElement;
        }
    }
}