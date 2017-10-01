using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Federation.Protocols.Request;
using Federation.Protocols.Request.Elements;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    public class Saml2SecurityTokenHandler : System.IdentityModel.Tokens.Saml2SecurityTokenHandler, ITokenHandler
    {
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
        
        internal XmlDocument GetPlainTestAsertion(XmlElement el)
        {
            var encryptedDataElement = GetElement(Federation.Protocols.Request.Elements.Xenc.EncryptedData.ElementName, Saml20Constants.Xenc, el);

            var encryptedData = new System.Security.Cryptography.Xml.EncryptedData();
            encryptedData.LoadXml(encryptedDataElement);
            var encryptedKey = new System.Security.Cryptography.Xml.EncryptedKey();
            var encryptedKeyElement = GetElement(Federation.Protocols.Request.Elements.Xenc.EncryptedKey.ElementName, Saml20Constants.Xenc, el);

            encryptedKey.LoadXml(encryptedKeyElement);
            var securityKeyIdentifier = new SecurityKeyIdentifier();
            foreach (KeyInfoX509Data v in encryptedKey.KeyInfo)
            {
                foreach (X509Certificate2 cert in v.Certificates)
                {
                    var cl = new X509RawDataKeyIdentifierClause(cert);
                    securityKeyIdentifier.Add(cl);
                }
            }

            var clause = new EncryptedKeyIdentifierClause(encryptedKey.CipherData.CipherValue, encryptedKey.EncryptionMethod.KeyAlgorithm, securityKeyIdentifier);
            SecurityKey key;
            var success = base.Configuration.ServiceTokenResolver.TryResolveSecurityKey(clause, out key);
            if (!success)
                throw new InvalidOperationException("Cannot locate security key");

            SymmetricSecurityKey symmetricSecurityKey = key as SymmetricSecurityKey;
            if (symmetricSecurityKey == null)
                throw new InvalidOperationException("Key must be symmentric key");

            SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedData.EncryptionMethod.KeyAlgorithm);
            var encryptedXml = new System.Security.Cryptography.Xml.EncryptedXml();
            
            var plaintext = encryptedXml.DecryptData(encryptedData, symmetricAlgorithm);
            var assertion = new XmlDocument { PreserveWhitespace = true };

            assertion.Load(new StringReader(Encoding.UTF8.GetString(plaintext)));
            return assertion;
        }

        private static XmlElement GetElement(string element, string elementNS, XmlElement doc)
        {
            var list = doc.GetElementsByTagName(element, elementNS);
            return list.Count == 0 ? null : (XmlElement)list[0];
        }
    }
}
