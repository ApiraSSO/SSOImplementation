namespace Shared.Federtion.Constants
{
    /// <summary>
    /// Status codes
    /// </summary>
    public static class StatusCodes
    {
        /// <summary>
        /// The request succeeded.
        /// </summary>
        public const string Success = "urn:oasis:names:tc:SAML:2.0:status:Success";

        /// <summary>
        /// The request could not be performed due to an error on the part of the requester.
        /// </summary>
        public const string Requester = "urn:oasis:names:tc:SAML:2.0:status:Requester";

        /// <summary>
        /// The request could not be performed due to an error on the part of the SAML responder or SAML authority.
        /// </summary>
        public const string Responder = "urn:oasis:names:tc:SAML:2.0:status:Responder";

        /// <summary>
        /// The SAML responder could not process the request because the version of the request message was incorrect.
        /// </summary>
        public const string VersionMismatch = "urn:oasis:names:tc:SAML:2.0:status:VersionMismatch";

        /// <summary>
        /// The responding provider was unable to successfully authenticate the principal.
        /// </summary>
        public const string AuthnFailed = "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed";

        /// <summary>
        /// Unexpected or invalid content was encountered within a <c>&lt;saml:Attribute&gt;</c> or <c>&lt;saml:AttributeValue&gt;</c> element.
        /// </summary>
        public const string InvalidAttrNameOrValue = "urn:oasis:names:tc:SAML:2.0:status:InvalidAttrNameOrValue";

        /// <summary>
        /// The responding provider cannot or will not support the requested name identifier policy.
        /// </summary>
        public const string InvalidNameIdPolicy = "urn:oasis:names:tc:SAML:2.0:status:InvalidNameIDPolicy";

        /// <summary>
        /// The specified authentication context requirements cannot be met by the responder.
        /// </summary>
        public const string NoAuthnContext = "urn:oasis:names:tc:SAML:2.0:status:NoAuthnContext";

        /// <summary>
        /// Used by an intermediary to indicate that none of the supported identity provider <c>&lt;Loc&gt;</c> elements in an
        /// <c>&lt;IDPList&gt;</c> can be resolved or that none of the supported identity providers are available.
        /// </summary>
        public const string NoAvailableIdp = "urn:oasis:names:tc:SAML:2.0:status:NoAvailableIDP";

        /// <summary>
        /// Indicates the responding provider cannot authenticate the principal passively, as has been requested.
        /// </summary>
        public const string NoPassive = "urn:oasis:names:tc:SAML:2.0:status:NoPassive";

        /// <summary>
        /// Used by an intermediary to indicate that none of the identity providers in an <c>&lt;IDPList&gt;</c> are
        /// supported by the intermediary.
        /// </summary>
        public const string NoSupportedIdp = "urn:oasis:names:tc:SAML:2.0:status:NoSupportedIDP";

        /// <summary>
        /// Used by a session authority to indicate to a session participant that it was not able to propagate logout
        /// to all other session participants.
        /// </summary>
        public const string PartialLogout = "urn:oasis:names:tc:SAML:2.0:status:PartialLogout";

        /// <summary>
        /// Indicates that a responding provider cannot authenticate the principal directly and is not permitted to
        /// proxy the request further.
        /// </summary>
        public const string ProxyCountExceeded = "urn:oasis:names:tc:SAML:2.0:status:ProxyCountExceeded";

        /// <summary>
        /// The SAML responder or SAML authority is able to process the request but has chosen not to respond.
        /// This status code MAY be used when there is concern about the security context of the request
        /// message or the sequence of request messages received from a particular requester.
        /// </summary>
        public const string RequestDenied = "urn:oasis:names:tc:SAML:2.0:status:RequestDenied";

        /// <summary>
        /// The SAML responder or SAML authority does not support the request.
        /// </summary>
        public const string RequestUnsupported = "urn:oasis:names:tc:SAML:2.0:status:RequestUnsupported";

        /// <summary>
        /// The SAML responder cannot process any requests with the protocol version specified in the request.
        /// </summary>
        public const string RequestVersionDeprecated = "urn:oasis:names:tc:SAML:2.0:status:RequestVersionDeprecated";

        /// <summary>
        /// The SAML responder cannot process the request because the protocol version specified in the
        /// request message is a major upgrade from the highest protocol version supported by the responder.
        /// </summary>
        public const string RequestVersionTooHigh = "urn:oasis:names:tc:SAML:2.0:status:RequestVersionTooHigh";

        /// <summary>
        /// The SAML responder cannot process the request because the protocol version specified in the
        /// request message is too low.
        /// </summary>
        public const string RequestVersionTooLow = "urn:oasis:names:tc:SAML:2.0:status:RequestVersionTooLow";

        /// <summary>
        /// The resource value provided in the request message is invalid or unrecognized.
        /// </summary>
        public const string ResourceNotRecognized = "urn:oasis:names:tc:SAML:2.0:status:ResourceNotRecognized";

        /// <summary>
        /// The response message would contain more elements than the SAML responder is able to return.
        /// </summary>
        public const string TooManyResponses = "urn:oasis:names:tc:SAML:2.0:status:TooManyResponses";

        /// <summary>
        /// An entity that has no knowledge of a particular attribute profile has been presented with an attribute
        /// drawn from that profile.
        /// </summary>
        public const string UnknownAttrProfile = "urn:oasis:names:tc:SAML:2.0:status:UnknownAttrProfile";

        /// <summary>
        /// The responding provider does not recognize the principal specified or implied by the request.
        /// </summary>
        public const string UnknownPrincipal = "urn:oasis:names:tc:SAML:2.0:status:UnknownPrincipal";

        /// <summary>
        /// The SAML responder cannot properly fulfill the request using the protocol binding specified in the
        /// request.
        /// </summary>
        public const string UnsupportedBinding = "urn:oasis:names:tc:SAML:2.0:status:UnsupportedBinding";
    }
}