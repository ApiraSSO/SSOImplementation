using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Kernel.Extensions;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;
using Kernel.Federation.Protocols.Response;
using Kernel.Federation.Tokens;
using Kernel.Logging;
using Shared.Federtion.Constants;

namespace Federation.Protocols.Response
{
    internal class ResponseHandler : IReponseHandler<ClaimsIdentity>
    {
        private readonly IRelayStateHandler _relayStateHandler;
        private readonly ITokenHandler _tokenHandler;
        
        public ResponseHandler(IRelayStateHandler relayStateHandler, ITokenHandler tokenHandler)
        {
            this._relayStateHandler = relayStateHandler;
            this._tokenHandler = tokenHandler;
        }
        public async Task<ClaimsIdentity> Handle(HttpPostResponseContext context)
        {
            //ToDo handle this properly, response handling, token validation, claims generation etc
            var elements = context.Form;
            var responseBase64 = elements[HttpRedirectBindingConstants.SamlResponse];
            var responseBytes = Convert.FromBase64String(responseBase64);
            var responseText = Encoding.UTF8.GetString(responseBytes);
            
            var relayState = await this._relayStateHandler.GetRelayStateFromFormData(elements);

            LoggerManager.WriteInformationToEventLog(String.Format("Response recieved:\r\n {0}", responseText));

            var responseStatus = ResponseHelper.ParseResponseStatus(responseText);
            ResponseHelper.EnsureSuccessAndThrow(responseStatus);

            using (var reader = new StringReader(responseText))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    var handlerContext = new HandleTokenContext(xmlReader, relayState, context.AuthenticationMethod);
                    var response = await this._tokenHandler.HandleToken(handlerContext);
                    if (!response.IsValid)
                        throw new Exception(EnumerableExtensions.Aggregate(response.ValidationResults.Select(x => x.ErrorMessage)));
                    return response.Identity;
                }
            }
        }
    }
}