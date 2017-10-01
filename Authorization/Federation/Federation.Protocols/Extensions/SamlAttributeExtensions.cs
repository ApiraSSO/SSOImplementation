using System;
using System.Collections.Generic;

using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Federation.Protocols.Extensions
{
    internal static class SamlAttributeExtensions
    {
       

        public static IEnumerable<Claim> ToClaims(this Saml2Attribute value, string issuer)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return new[] { new Claim(value.Name, value.Name, issuer) };
        }
    }
}