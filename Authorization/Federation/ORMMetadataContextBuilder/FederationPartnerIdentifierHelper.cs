using System.Web;

namespace ORMMetadataContextProvider
{
    internal class FederationPartnerIdentifierHelper
    {
        internal static string GetFederationPartyIdFromRequestOrDefault()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
                return "local";
            var querySting = HttpContext.Current.Request.QueryString;
            var federationPartyId = querySting["clientId"];
            return federationPartyId ?? "local";
        }
    }
}