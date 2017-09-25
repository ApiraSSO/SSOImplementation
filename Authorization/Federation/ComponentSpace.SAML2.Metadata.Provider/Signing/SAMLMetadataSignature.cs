using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using ComponentSpace.Saml2.Exceptions;

namespace ComponentSpace.SAML2.Metadata.Provider.Signing
{
    public static class SAMLMetadataSignature
    {
        public const string DefaultInclusiveNamespacesPrefixList = "#default md saml ds xs xsi";

        static SAMLMetadataSignature()
        {
            
        }

        private static string GetID(XmlElement xmlElement)
        {
            string str = xmlElement.GetAttribute("ID").Trim();
            if (string.IsNullOrEmpty(str))
                throw new SamlSignatureException("The ID is missing.");
            return str;
        }

        private static void AddSignature(XmlElement xmlElement, XmlElement xmlSignature)
        {
            xmlElement.PrependChild((XmlNode)xmlSignature);
        }

        public static bool IsSigned(XmlElement xmlElement)
        {
            return XmlSignature.IsSigned(xmlElement);
        }

        public static void RemoveSignature(XmlElement xmlElement)
        {
            XmlSignature.RemoveSignature(xmlElement);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, KeyInfo keyInfo, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            if (inclusiveNamespacesPrefixList == null)
                inclusiveNamespacesPrefixList = "#default md saml ds xs xsi";
            XmlElement xmlSignature = XmlSignature.Generate(xmlElement, SAMLMetadataSignature.GetID(xmlElement), signingKey, keyInfo, (SignedXml)new SignedMetadata(xmlElement), inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
            SAMLMetadataSignature.AddSignature(xmlElement, xmlSignature);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, KeyInfo keyInfo)
        {
            SAMLMetadataSignature.Generate(xmlElement, signingKey, keyInfo, "#default md saml ds xs xsi", (string)null, (string)null);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, KeyInfoX509Data keyInfoX509Data, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            if (inclusiveNamespacesPrefixList == null)
                inclusiveNamespacesPrefixList = "#default md saml ds xs xsi";
            XmlElement xmlSignature = XmlSignature.Generate(xmlElement, SAMLMetadataSignature.GetID(xmlElement), signingKey, keyInfoX509Data, (SignedXml)new SignedMetadata(xmlElement), inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
            SAMLMetadataSignature.AddSignature(xmlElement, xmlSignature);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, KeyInfoX509Data keyInfoX509Data)
        {
            SAMLMetadataSignature.Generate(xmlElement, signingKey, keyInfoX509Data, "#default md saml ds xs xsi", (string)null, (string)null);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, X509Certificate2Collection x509Certificates, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            if (inclusiveNamespacesPrefixList == null)
                inclusiveNamespacesPrefixList = "#default md saml ds xs xsi";
            XmlElement xmlSignature = XmlSignature.Generate(xmlElement, SAMLMetadataSignature.GetID(xmlElement), signingKey, x509Certificates, (SignedXml)new SignedMetadata(xmlElement), inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
            SAMLMetadataSignature.AddSignature(xmlElement, xmlSignature);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, X509Certificate2Collection x509Certificates)
        {
            SAMLMetadataSignature.Generate(xmlElement, signingKey, x509Certificates, "#default md saml ds xs xsi", (string)null, (string)null);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, X509Certificate2 x509Certificate, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            if (inclusiveNamespacesPrefixList == null)
                inclusiveNamespacesPrefixList = "#default md saml ds xs xsi";
            XmlElement xmlSignature = XmlSignature.Generate(xmlElement, SAMLMetadataSignature.GetID(xmlElement), signingKey, x509Certificate, (SignedXml)new SignedMetadata(xmlElement), inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
            SAMLMetadataSignature.AddSignature(xmlElement, xmlSignature);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, X509Certificate2 x509Certificate)
        {
            SAMLMetadataSignature.Generate(xmlElement, signingKey, x509Certificate, "#default md saml ds xs xsi", (string)null, (string)null);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod)
        {
            if (inclusiveNamespacesPrefixList == null)
                inclusiveNamespacesPrefixList = "#default md saml ds xs xsi";
            XmlElement xmlSignature = XmlSignature.Generate(xmlElement, SAMLMetadataSignature.GetID(xmlElement), signingKey, (SignedXml)new SignedMetadata(xmlElement), inclusiveNamespacesPrefixList, digestMethod, signatureMethod);
            SAMLMetadataSignature.AddSignature(xmlElement, xmlSignature);
        }

        public static void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey)
        {
            SAMLMetadataSignature.Generate(xmlElement, signingKey, "#default md saml ds xs xsi", (string)null, (string)null);
        }

        public static bool Verify(XmlElement xmlElement, KeyInfo keyInfo)
        {
            return XmlSignature.Verify(xmlElement, keyInfo, (SignedXml)new SignedMetadata(xmlElement));
        }

        public static bool Verify(XmlElement xmlElement, KeyInfoX509Data keyInfoX509Data)
        {
            return XmlSignature.Verify(xmlElement, keyInfoX509Data, (SignedXml)new SignedMetadata(xmlElement));
        }

        public static bool Verify(XmlElement xmlElement, X509Certificate2 x509Certificate)
        {
            return XmlSignature.Verify(xmlElement, x509Certificate, (SignedXml)new SignedMetadata(xmlElement));
        }

        public static bool Verify(XmlElement xmlElement, AsymmetricAlgorithm signingKey)
        {
            return XmlSignature.Verify(xmlElement, signingKey, (SignedXml)new SignedMetadata(xmlElement));
        }

        public static bool Verify(XmlElement xmlElement)
        {
            return XmlSignature.Verify(xmlElement, (SignedXml)new SignedMetadata(xmlElement));
        }

        public static KeyInfo GetKeyInfo(XmlElement xmlElement)
        {
            return XmlSignature.GetKeyInfo(xmlElement);
        }

        public static X509Certificate2 GetCertificate(XmlElement xmlElement)
        {
            return XmlSignature.GetCertificate(xmlElement);
        }
    }
}