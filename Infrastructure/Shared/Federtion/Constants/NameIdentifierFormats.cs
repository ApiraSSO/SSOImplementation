namespace Shared.Federtion.Constants
{
    /// <summary>
    /// Formats of name identifier formats
    /// </summary>
    public static class NameIdentifierFormats
    {
        /// <summary>
        /// urn for Unspecified name identifier format
        /// </summary>
        public const string Unspecified = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";

        /// <summary>
        /// urn for Email name identifier format
        /// </summary>
        public const string Email = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";

        /// <summary>
        /// urn for X509SubjectName name identifier format
        /// </summary>
        public const string X509SubjectName = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";

        /// <summary>
        /// urn for Windows name identifier format
        /// </summary>
        public const string Windows = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";

        /// <summary>
        /// urn for Kerberos name identifier format
        /// </summary>
        public const string Kerberos = "urn:oasis:names:tc:SAML:2.0:nameid-format:kerberos";

        /// <summary>
        /// urn for Entity name identifier format
        /// </summary>
        public const string Entity = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";

        /// <summary>
        /// urn for Persistent name identifier format
        /// </summary>
        public const string Persistent = "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";

        /// <summary>
        /// urn for Transient name identifier format
        /// </summary>
        public const string Transient = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient";

        public const string Encrypted = "urn:oasis:names:tc:SAML:2.0:nameid-format:encrypted";
    }
}