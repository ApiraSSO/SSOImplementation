using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;
using Kernel.Federation.Protocols.Response;
using Kernel.Federation.Tokens;

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
            var responseBase64 = elements["SAMLResponse"];
            var responseBytes = Convert.FromBase64String(responseBase64);
            var responseText = Encoding.UTF8.GetString(responseBytes);
            
            var relayState = await this._relayStateHandler.GetRelayStateFromFormData(elements);
#if(DEBUG)
            this.SaveTemp(responseText);
#endif
            using (var reader = new StringReader(responseText))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    this.ValidateResponseSuccess(xmlReader);
                }
            }
            using (var reader = new StringReader(responseText))
            {
                using (var xmlReader = XmlReader.Create(reader))
                {
                    var handlerContext = new HandleTokenContext(xmlReader, relayState, context.AuthenticationMethod);
                    var response = await this._tokenHandler.HandleToken(handlerContext);
                    return response.Identity;
                }
            }
        }

        //ToDo: sort this out clean up
        private void ValidateResponseSuccess(XmlReader reader)
        {
            while (!reader.IsStartElement("StatusCode", "urn:oasis:names:tc:SAML:2.0:protocol"))
            {
                if (!reader.Read())
                    throw new InvalidOperationException("Can't find status code element.");
            }
            var status = reader.GetAttribute("Value");
            if (String.IsNullOrWhiteSpace(status) || !String.Equals(status, "urn:oasis:names:tc:SAML:2.0:status:Success"))
                throw new Exception(status);
        }
        //ToDo clean up
        private void SaveTemp(string responseText)
        {
            try
            {
                var path = @"D:\Dan\Software\Apira\Assertions\";
                var now = DateTimeOffset.Now;
                var tag = String.Format("{0}{1}{2}{3}{4}", now.Year, now.Month, now.Day, now.Hour, now.Minute);
                var writer = XmlWriter.Create(String.Format("{0}{1}{2}", path, tag, ".xml"));
                var el = new XmlDocument();
                el.Load(new StringReader(responseText));
                el.DocumentElement.WriteTo(writer);
                writer.Flush();
                writer.Dispose();
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}