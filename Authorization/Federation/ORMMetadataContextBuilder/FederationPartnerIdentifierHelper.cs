using System.Web;

namespace ORMMetadataContextProvider
{
    internal class FederationPartnerIdentifierHelper
    {
        internal static string GetRelyingPartyIdFromRequestOrDefault()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
                return "local";
            var querySting = HttpContext.Current.Request.QueryString;
            var relyingPartyId = querySting["clientId"];
            return relyingPartyId ?? "local";
        }
    }
}
