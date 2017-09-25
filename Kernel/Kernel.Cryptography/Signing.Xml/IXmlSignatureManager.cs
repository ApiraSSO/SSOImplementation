using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Kernel.Cryptography.Signing.Xml
{
    public interface IXmlSignatureManager
    {
        KeyInfo CreateKeyInfo(X509Certificate2 certificate);
        void Generate(XmlElement xmlElement, AsymmetricAlgorithm signingKey, X509Certificate2 x509Certificate, string inclusiveNamespacesPrefixList, string digestMethod, string signatureMethod);
    }
}