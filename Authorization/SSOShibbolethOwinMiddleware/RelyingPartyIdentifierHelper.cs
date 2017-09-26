using System;
using Microsoft.Owin;

namespace SSOOwinMiddleware
{
    internal class RelyingPartyIdentifierHelper
    {
        internal static string GetRelyingPartyIdFromRequestOrDefault(IOwinContext context)
        {
            if (context == null)
                throw new ArgumentNullException("owinContext");
            if (context.Request == null)
                throw new ArgumentNullException("http request");
            var querySting = context.Request.Query;
            var relyingPartyId = querySting["clientId"];
            return relyingPartyId ?? "local";
        }
    }
}