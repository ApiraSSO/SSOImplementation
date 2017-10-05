using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Tokens
{
    internal class TokenHelper
    {
        internal static XmlDocument GetPlainAsertion(SecurityTokenResolver securityTokenResolver, XmlElement el)
        {
            var encryptedDataElement = GetElement(HttpRedirectBindingConstants.EncryptedData, Saml20Constants.Xenc, el);

            var encryptedData = new System.Security.Cryptography.Xml.EncryptedData();
            encryptedData.LoadXml(encryptedDataElement);
            var encryptedKey = new System.Security.Cryptography.Xml.EncryptedKey();
            var encryptedKeyElement = GetElement(HttpRedirectBindingConstants.EncryptedKey, Saml20Constants.Xenc, el);

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
            var success = securityTokenResolver.TryResolveSecurityKey(clause, out key);
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

        internal static bool VerifySignature(XmlElement el)
        {
            var cspParams = new CspParameters();
            cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
            var rsaKey = new RSACryptoServiceProvider(cspParams);
            
            var assertionElement = el.LocalName == "Assertion" ? el : TokenHelper.GetElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion", el);
            var signEl = TokenHelper.GetElement("Signature", "http://www.w3.org/2000/09/xmldsig#", el);
            var certEl = TokenHelper.GetElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#", signEl);

            var signedXml = new SignedXml(assertionElement);

            var dcert2 = new X509Certificate2(Convert.FromBase64String(certEl.InnerText));

            signedXml.LoadXml(signEl);
            var valid = signedXml.CheckSignature(dcert2, true);
            return valid;
        }

        internal static XmlElement GetElement(string element, string elementNS, XmlElement doc)
        {
            var list = doc.GetElementsByTagName(element, elementNS);
            return list.Count == 0 ? null : (XmlElement)list[0];
        }

        internal static void MoveToToken(XmlReader reader)
        {
            while (!(reader.IsStartElement(HttpRedirectBindingConstants.EncryptedAssertion, Saml20Constants.Assertion) || reader.IsStartElement("Assertion", Saml20Constants.Assertion)))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find assertion element.");
            }
        }
    }
}