using System;
using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SecurityManagement.Exceptions;

namespace SecurityManagement.DefaultSigning
{
    internal static class XmlSignature
    {
        private const string keyInfoXPath = "//*[local-name(.) = 'KeyInfo' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']";
        private const string signatureXPath = "*[local-name(.) = 'Signature' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']";
        private const string x509CertificateXPath = "//*[local-name(.) = 'X509Certificate' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']";

        static XmlSignature()
        {
        }

        private static AsymmetricAlgorithm GetSigningKeyFromKeyInfo(KeyInfo keyInfo)
        {
            AsymmetricAlgorithm asymmetricAlgorithm = (AsymmetricAlgorithm)null;
            IEnumerator enumerator = keyInfo.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is KeyInfoX509Data)
                {
                    KeyInfoX509Data current = (KeyInfoX509Data)enumerator.Current;
                    if (current.Certificates.Count != 0)
                    {
                        asymmetricAlgorithm = new X509Certificate2((X509Certificate)current.Certificates[0]).PublicKey.Key;
                        break;
                    }
                }
                else
                {
                    if (enumerator.Current is RSAKeyValue)
                    {
                        asymmetricAlgorithm = (AsymmetricAlgorithm)((RSAKeyValue)enumerator.Current).Key;
                        break;
                    }
                    if (enumerator.Current is DSAKeyValue)
                    {
                        asymmetricAlgorithm = (AsymmetricAlgorithm)((DSAKeyValue)enumerator.Current).Key;
                        break;
                    }
                }
            }
            return asymmetricAlgorithm;
        }

        public static XmlElement PrepareForSignatureVerification(XmlElement xmlElement)
        {
            throw new NotImplementedException();
            //IDictionary<string, string> namespacesInScope = xmlElement.CreateNavigator().GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
            //foreach (string key in (IEnumerable<string>)namespacesInScope.Keys)
            //{
            //    if (string.IsNullOrEmpty(key))
            //        xmlElement.SetAttribute("xmlns", namespacesInScope[key]);
            //    else
            //        xmlElement.SetAttribute(string.Format("xmlns:{0}", (object)key), namespacesInScope[key]);
            //}
            //XmlDocument document = XmlHelper.CreateDocument();
            //document.AppendChild(document.ImportNode((XmlNode)xmlElement, true));
            //return document.DocumentElement;
        }

        public static XmlElement GetSignatureElement(XmlElement xmlElement)
        {
            return (XmlElement)xmlElement.SelectSingleNode("*[local-name(.) = 'Signature' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']");
        }

        public static XmlElement GetKeyInfoElement(XmlElement xmlElement)
        {
            XmlElement signatureElement = XmlSignature.GetSignatureElement(xmlElement);
            if (signatureElement == null)
                return (XmlElement)null;
            return (XmlElement)signatureElement.SelectSingleNode("//*[local-name(.) = 'KeyInfo' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']");
        }

        public static XmlElement GetCertificateElement(XmlElement xmlElement)
        {
            XmlElement signatureElement = XmlSignature.GetSignatureElement(xmlElement);
            if (signatureElement == null)
                return (XmlElement)null;
            return (XmlElement)signatureElement.SelectSingleNode("//*[local-name(.) = 'X509Certificate' and namespace-uri(.) = 'http://www.w3.org/2000/09/xmldsig#']");
        }

        public static bool IsSigned(XmlElement xmlElement)
        {
            return XmlSignature.GetSignatureElement(xmlElement) != null;
        }

        public static void RemoveSignature(XmlElement xmlElement)
        {
            XmlElement signatureElement = XmlSignature.GetSignatureElement(xmlElement);
            if (signatureElement == null)
                return;
            xmlElement.RemoveChild((XmlNode)signatureElement);
        }

        public static XmlElement Generate(XmlElement xmlElement, string elementId, AsymmetricAlgorithm signingKey, KeyInfo keyInfo, SignedXml signedXml, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            try
            {
                XmlSignature.RemoveSignature(xmlElement);
                signedXml.SigningKey = signingKey;
                signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
                Reference reference = new Reference();
                reference.Uri = "#" + elementId;
                reference.AddTransform((Transform)new XmlDsigEnvelopedSignatureTransform());
                if (!string.IsNullOrEmpty(digestMethod))
                    reference.DigestMethod = digestMethod;
                XmlDsigExcC14NTransform excC14Ntransform = new XmlDsigExcC14NTransform();
                //if (!string.IsNullOrEmpty(inclusiveNamespacesPrefixList))
                //    excC14Ntransform.InclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;
                reference.AddTransform((Transform)excC14Ntransform);
                signedXml.AddReference(reference);
                signedXml.KeyInfo = keyInfo;
                if (!string.IsNullOrEmpty(signatureMethod))
                    signedXml.SignedInfo.SignatureMethod = signatureMethod;
                signedXml.ComputeSignature();
                return signedXml.GetXml();
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to generate XML signature.", ex);
            }
        }

        public static XmlElement Generate(XmlElement xmlElement, string elementId, AsymmetricAlgorithm signingKey, KeyInfoX509Data keyInfoX509Data, SignedXml signedXml, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            KeyInfo keyInfo;
            try
            {
                keyInfo = new KeyInfo();
                keyInfo.AddClause((KeyInfoClause)keyInfoX509Data);
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to create key info using X.509 data.", ex);
            }
            return XmlSignature.Generate(xmlElement, elementId, signingKey, keyInfo, signedXml, inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
        }

        public static XmlElement Generate(XmlElement xmlElement, string elementId, AsymmetricAlgorithm signingKey, X509Certificate2Collection x509Certificates, SignedXml signedXml, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            KeyInfo keyInfo;
            try
            {
                keyInfo = new KeyInfo();
                KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data();
                foreach (X509Certificate2 x509Certificate in x509Certificates)
                    keyInfoX509Data.AddCertificate((X509Certificate)x509Certificate);
                keyInfo.AddClause((KeyInfoClause)keyInfoX509Data);
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to create key info using X.509 certificate.", ex);
            }
            return XmlSignature.Generate(xmlElement, elementId, signingKey, keyInfo, signedXml, inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
        }

        public static XmlElement Generate(XmlElement xmlElement, string elementId, AsymmetricAlgorithm signingKey, X509Certificate2 x509Certificate, SignedXml signedXml, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            X509Certificate2Collection x509Certificates;
            try
            {
                x509Certificates = new X509Certificate2Collection();
                x509Certificates.Add(x509Certificate);
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to create X.509 certificate collection.", ex);
            }
            return XmlSignature.Generate(xmlElement, elementId, signingKey, x509Certificates, signedXml, inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
        }

        public static XmlElement Generate(XmlElement xmlElement, string elementId, AsymmetricAlgorithm signingKey, SignedXml signedXml, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            return XmlSignature.Generate(xmlElement, elementId, signingKey, new KeyInfo(), signedXml, inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
        }

        public static bool Verify(XmlElement xmlElement, KeyInfo keyInfo, SignedXml signedXml)
        {
            AsymmetricAlgorithm signingKeyFromKeyInfo = XmlSignature.GetSigningKeyFromKeyInfo(keyInfo);
            if (signingKeyFromKeyInfo == null)
                throw new SamlSignatureException("No signing key could be found in the key info.");
            return XmlSignature.Verify(xmlElement, signingKeyFromKeyInfo, signedXml);
        }

        public static bool Verify(XmlElement xmlElement, KeyInfoX509Data keyInfoX509Data, SignedXml signedXml)
        {
            if (keyInfoX509Data.Certificates.Count == 0)
                throw new SamlSignatureException("No X.509 certificates in key info.");
            X509Certificate2 x509Certificate = new X509Certificate2((X509Certificate)keyInfoX509Data.Certificates[0]);
            return XmlSignature.Verify(xmlElement, x509Certificate, signedXml);
        }

        public static bool Verify(XmlElement xmlElement, X509Certificate2 x509Certificate, SignedXml signedXml)
        {
            return XmlSignature.Verify(xmlElement, x509Certificate != null ? x509Certificate.PublicKey.Key : (AsymmetricAlgorithm)null, signedXml);
        }

        public static bool Verify(XmlElement xmlElement, AsymmetricAlgorithm signingKey, SignedXml signedXml)
        {
            try
            {
                XmlElement signatureElement = XmlSignature.GetSignatureElement(xmlElement);
                if (signatureElement == null)
                    throw new SamlSignatureException("The XML does not contain a signature.");
                signedXml.LoadXml(signatureElement);
                return signingKey == null ? signedXml.CheckSignature() : signedXml.CheckSignature(signingKey);
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to verify the XML signature.", ex);
            }
        }

        public static bool Verify(XmlElement xmlElement, SignedXml signedXml)
        {
            return XmlSignature.Verify(xmlElement, (AsymmetricAlgorithm)null, signedXml);
        }

        public static KeyInfo GetKeyInfo(XmlElement xmlElement)
        {
            try
            {
                XmlElement keyInfoElement = XmlSignature.GetKeyInfoElement(xmlElement);
                if (keyInfoElement == null)
                    return (KeyInfo)null;
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.LoadXml(keyInfoElement);
                return keyInfo;
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to extract key info from XML.", ex);
            }
        }

        public static X509Certificate2 GetCertificate(XmlElement xmlElement)
        {
            try
            {
                XmlElement certificateElement = XmlSignature.GetCertificateElement(xmlElement);
                if (certificateElement == null)
                    return (X509Certificate2)null;
                string s = certificateElement.InnerText.Trim();
                if (string.IsNullOrEmpty(s))
                    return (X509Certificate2)null;
                return new X509Certificate2(Convert.FromBase64String(s));
            }
            catch (Exception ex)
            {
                throw new SamlSignatureException("Failed to extract X.509 certificate from XML.", ex);
            }
        }

        public static class Prefixes
        {
            public const string DS = "ds";
        }

        public static class NamespaceURIs
        {
            public const string XmlDSig = "http://www.w3.org/2000/09/xmldsig#";
        }

        public static class ElementNames
        {
            public const string KeyInfo = "KeyInfo";
            public const string Signature = "Signature";
            public const string X509Certificate = "X509Certificate";
        }
    }
}