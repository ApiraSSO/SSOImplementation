using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Federation.Protocols.Extensions
{
    internal static class SamlAttributeExtensions
    {
       

        public static IEnumerable<Claim> ToClaims(this Saml2Attribute attribute, string issuer)
        {
            if (attribute == null)
                throw new ArgumentNullException("value");
            return attribute.Values.Select(x => new Claim(attribute.FriendlyName, x, attribute.AttributeValueXsiType, issuer));
        }
    }
}