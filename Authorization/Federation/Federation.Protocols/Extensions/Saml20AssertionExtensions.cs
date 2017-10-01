using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Federation.Protocols.Extensions
{
    internal static class Saml20AssertionExtensions
    {
        public static ClaimsIdentity ToClaimsIdentity(this Saml2Assertion value, string authenticationType, string nameType = null, string roleType = null)
        {
            //throw new NotImplementedException();
            if (value == null) throw new ArgumentNullException("value");
            var claims = value.Statements
                .OfType<Saml2AttributeStatement>()
                .SelectMany(a => a.Attributes, (s, atr) => atr.ToClaims(value.Issuer.Value))
                .SelectMany(r => r)
                .Union(ClaimsFromSubject(value.Subject, value.Issuer));
            return new ClaimsIdentity(claims
                , authenticationType, nameType, roleType);
        }

        private static IEnumerable<Claim> ClaimsFromSubject(Saml2Subject subject, Saml2NameIdentifier issuer)
        {
            if (subject == null)
                yield break;
            if (subject.NameId != null)
            {
                yield return new Claim("sub", subject.NameId.Value, ClaimValueTypes.String, issuer.Value); // openid connect
                yield return new Claim(ClaimTypes.NameIdentifier, subject.NameId.Value, ClaimValueTypes.String, issuer.Value); // saml
            }
        }
    }
}
