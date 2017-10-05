namespace Shared.Federtion.Constants
{
    /// <summary>
    /// Logout reasons
    /// </summary>
    public static class Reasons
    {
        /// <summary>
        /// Specifies that the message is being sent because the principal wishes to terminate the indicated session.
        /// </summary>
        public const string User = "urn:oasis:names:tc:SAML:2.0:logout:user";

        /// <summary>
        /// Specifies that the message is being sent because an administrator wishes to terminate the indicated
        /// session for that principal.
        /// </summary>
        public const string Admin = "urn:oasis:names:tc:SAML:2.0:logout:admin";
    }
}
