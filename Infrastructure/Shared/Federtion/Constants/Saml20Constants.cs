namespace Shared.Federtion.Constants
{
    /// <summary>
    /// Constants related to SAML 2.0
    /// </summary>
    public class Saml20Constants
    {
        /// <summary>
        /// SAML Version
        /// </summary>
        public const string Version = "2.0";

        /// <summary>
        /// The XML namespace of the SAML 2.0 assertion schema.
        /// </summary>
        public const string Assertion = "urn:oasis:names:tc:SAML:2.0:assertion";

        /// <summary>
        /// The XML namespace of the SAML 2.0 protocol schema
        /// </summary>
        public const string Protocol = "urn:oasis:names:tc:SAML:2.0:protocol";

        /// <summary>
        /// The XML namespace of the SAML 2.0 metadata schema
        /// </summary>
        public const string Metadata = "urn:oasis:names:tc:SAML:2.0:metadata";

        /// <summary>
        /// The XML namespace of <c>XmlDSig</c>
        /// </summary>
        public const string Xmldsig = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        /// The XML namespace of <c>XmlEnc</c>
        /// </summary>
        public const string Xenc = "http://www.w3.org/2001/04/xmlenc#";

        /// <summary>
        /// The default value of the Format property for a <c>NameID</c> element
        /// </summary>
        public const string DefaultNameIdFormat = "urn:oasis:names:tc:SAML:1.0:nameid-format:unspecified";

        /// <summary>
        /// The mime type that must be used when publishing a metadata document.
        /// </summary>
        public const string MetadataMimetype = "application/samlmetadata+xml";

        /// <summary>
        /// All the namespaces defined and reserved by the SAML 2.0 standard
        /// </summary>
        public static readonly string[] SamlNamespaces = { Assertion, Protocol, Metadata };
    }
}