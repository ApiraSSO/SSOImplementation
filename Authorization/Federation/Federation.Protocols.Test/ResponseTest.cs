using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Federation.Protocols.Request;
using Federation.Protocols.Request.Elements;
using Federation.Protocols.Request.Elements.Xenc;
using Federation.Protocols.Response;
using Federation.Protocols.Test.Mock;
using NUnit.Framework;

namespace Federation.Protocols.Test
{
    [TestFixture]
    internal class ResponseTest
    {
        [Test]
        public void T1()
        {
            //ARRANGE
           
            var doc = new XmlDocument();
            doc.Load(@"D:\Dan\Software\Apira\a.xml");
            var el = doc.DocumentElement;
            var encryptedList = el.GetElementsByTagName(EncryptedAssertion.ElementName, Saml20Constants.Assertion);
            if (encryptedList.Count == 1)
            {

                var encryptedAssertion = (XmlElement)encryptedList[0];
                //this.Decrypt(encryptedAssertion);
                var xmlReader = XmlReader.Create(new StringReader(encryptedAssertion.OuterXml));
                //var h = new Saml2SecurityTokenHandler();
                var h = new Saml2SecurityTokenHandlerMock();
                var inner = new X509CertificateStoreTokenResolver("TestCertStore", StoreLocation.LocalMachine);
                var tr = new IssuerTokenResolver(inner);
                
                h.Configuration = new SecurityTokenHandlerConfiguration
                {
                    IssuerTokenResolver = tr,
                    ServiceTokenResolver = tr
                    
                };
                var b = h.CanReadToken(xmlReader);
                var ass = h.GetAssertion(xmlReader);
                //var t = h.ReadToken(xmlReader);
                //ACT
                //ASSERT
            }


        }
        [Test]
        public void T2()
        {
            //ARRANGE
            var xmlReader = XmlReader.Create(@"D:\Dan\Software\Apira\a.xml");
            var reader = XmlReader.Create(xmlReader, xmlReader.Settings);
            this.ValidateResponseSuccess(xmlReader);
            //xmlReader.MoveToContent();
            //var innerXml = xmlReader.ReadOuterXml();
            //var element = xmlReader.ReadElementContentAsString();
            var tokenHandlerConfigurationProvider = new TokenHandlerConfigurationProvider();
            var saml2SecurityTokenHandler = new Federation.Protocols.Response.Saml2SecurityTokenHandler(tokenHandlerConfigurationProvider);
            var assertion = saml2SecurityTokenHandler.GetAssertion(reader);
            
        }

        private void ValidateResponseSuccess(XmlReader reader)
        {
            while (!reader.IsStartElement("StatusCode", "urn:oasis:names:tc:SAML:2.0:protocol"))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find assertion element.");
            }
            var status = reader.GetAttribute("Value");
            if (String.IsNullOrWhiteSpace(status) || !String.Equals(status, "urn:oasis:names:tc:SAML:2.0:status:Success"))
                throw new Exception(status);
        }

        private static XmlElement GetElement(string element, string elementNS, XmlElement doc)
        {
            var list = doc.GetElementsByTagName(element, elementNS);
            return list.Count == 0 ? null : (XmlElement)list[0];
        }

        public void Decrypt(XmlElement el)
        {

            var store = new X509Store("TestCertStore");
            store.Open(OpenFlags.ReadOnly);
            var cer = store.Certificates.Find(X509FindType.FindBySubjectName, "Apira_DevEnc", false)[0];
           

            var encryptedDataElement = GetElement(EncryptedData.ElementName, Saml20Constants.Xenc, el);
            var encryptedData = new System.Security.Cryptography.Xml.EncryptedData();
            encryptedData.LoadXml(encryptedDataElement);

            SymmetricAlgorithm sessionKey = null;
            if (encryptedData.EncryptionMethod != null)
            {
                var sessionKeyAlgorithm = encryptedData.EncryptionMethod.KeyAlgorithm;
                sessionKey = ExtractSessionKey(encryptedDataElement.OwnerDocument, sessionKeyAlgorithm, (RSA)cer.PrivateKey);
            }
            else
            {
                //sessionKey = ExtractSessionKey(encryptedDataElement.OwnerDocument);
            }

            /*
             * NOTE: 
             * The EncryptedXml class can't handle an <EncryptedData> element without an underlying <EncryptionMethod> element,
             * despite the standard dictating that this is ok. 
             * If this becomes a problem with other IDPs, consider adding a default EncryptionMethod instance manually before decrypting.
             */
            var encryptedXml = new System.Security.Cryptography.Xml.EncryptedXml();
            var plaintext = encryptedXml.DecryptData(encryptedData, sessionKey);

            var assertion = new XmlDocument { PreserveWhitespace = true };
            try
            {
                assertion.Load(new StringReader(Encoding.UTF8.GetString(plaintext)));
            }
            catch (XmlException e)
            {
                //Assertion = null;
                throw new Saml20FormatException("Unable to parse the decrypted assertion.", e);
            }
        }

        private SymmetricAlgorithm ExtractSessionKey(XmlDocument encryptedAssertionDoc, string keyAlgorithm, RSA pk)
        {
            // Check if there are any <EncryptedKey> elements immediately below the EncryptedAssertion element.
            foreach (XmlNode node in encryptedAssertionDoc.DocumentElement.ChildNodes)
            {
                if (node.LocalName == EncryptedKey.ElementName && node.NamespaceURI == Saml20Constants.Xenc)
                {
                    return ToSymmetricKey((XmlElement)node, keyAlgorithm, pk);
                }
            }

            // Check if the key is embedded in the <EncryptedData> element.
            var encryptedData = GetElement(EncryptedData.ElementName, Saml20Constants.Xenc, encryptedAssertionDoc.DocumentElement);
            if (encryptedData != null)
            {
                var encryptedKeyElement = GetElement(EncryptedKey.ElementName, Saml20Constants.Xenc, encryptedAssertionDoc.DocumentElement);
                if (encryptedKeyElement != null)
                {
                    return ToSymmetricKey(encryptedKeyElement, keyAlgorithm, pk);
                }
            }

            throw new Saml20FormatException("Unable to locate assertion decryption key.");
        }

        /// <summary>
        /// Extracts the key from a &lt;EncryptedKey&gt; element.
        /// </summary>
        /// <param name="encryptedKeyElement">The encrypted key element.</param>
        /// <param name="keyAlgorithm">The key algorithm.</param>
        /// <returns>The <see cref="SymmetricAlgorithm"/>.</returns>
        private SymmetricAlgorithm ToSymmetricKey(XmlElement encryptedKeyElement, string keyAlgorithm, RSA pk)
        {
            var encryptedKey = new System.Security.Cryptography.Xml.EncryptedKey();
            encryptedKey.LoadXml(encryptedKeyElement);

            var useOaep = false;//UseOaepDefault;
            if (encryptedKey.EncryptionMethod != null)
            {
                useOaep = encryptedKey.EncryptionMethod.KeyAlgorithm == System.Security.Cryptography.Xml.EncryptedXml.XmlEncRSAOAEPUrl;
            }

            if (encryptedKey.CipherData.CipherValue != null)
            {
                var key = GetKeyInstance(keyAlgorithm);
                
                key.Key = System.Security.Cryptography.Xml.EncryptedXml.DecryptKey(encryptedKey.CipherData.CipherValue, pk, useOaep);

                return key;
            }

            throw new NotImplementedException("Unable to decode CipherData of type \"CipherReference\".");
        }

        private static SymmetricAlgorithm GetKeyInstance(string algorithm)
        {
            SymmetricAlgorithm result;
            switch (algorithm)
            {
                case System.Security.Cryptography.Xml.EncryptedXml.XmlEncTripleDESUrl:
                    result = TripleDES.Create();
                    break;
                case System.Security.Cryptography.Xml.EncryptedXml.XmlEncAES128Url:
                    result = new RijndaelManaged { KeySize = 128 };
                    break;
                case System.Security.Cryptography.Xml.EncryptedXml.XmlEncAES192Url:
                    result = new RijndaelManaged { KeySize = 192 };
                    break;
                case System.Security.Cryptography.Xml.EncryptedXml.XmlEncAES256Url:
                default:
                    result = new RijndaelManaged { KeySize = 256 };
                    break;
            }

            return result;
        }
    }
}
