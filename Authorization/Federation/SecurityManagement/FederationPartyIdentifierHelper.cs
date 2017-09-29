using System;
using System.Net;
using System.Web;

namespace SecurityManagement
{
    internal class FederationPartyIdentifierHelper
    {
        internal static string GetFederationPartyIdFromRequestOrDefault(HttpWebRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("owinContext");
            
            var querySting = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var federationPartyId = querySting["clientId"];
            return federationPartyId ?? "local";
        }
    }
}