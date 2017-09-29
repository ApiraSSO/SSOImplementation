using System;
using Microsoft.Owin;

namespace SSOOwinMiddleware
{
    internal class FederationPartyIdentifierHelper
    {
        internal static string GetFederationPartyIdFromRequestOrDefault(IOwinContext context)
        {
            if (context == null)
                throw new ArgumentNullException("owinContext");
            if (context.Request == null)
                throw new ArgumentNullException("http request");
            var querySting = context.Request.Query;
            var federationPartyId = querySting["clientId"];
            return federationPartyId ?? "local";
        }
    }
}