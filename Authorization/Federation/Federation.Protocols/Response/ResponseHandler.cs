using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Kernel.Authentication.Claims;
using Kernel.Federation.FederationPartner;
using Kernel.Federation.Protocols;
using Kernel.Federation.Protocols.Bindings.HttpPostBinding;
using Kernel.Federation.Protocols.Response;

namespace Federation.Protocols.Response
{
    internal class ResponseHandler : IReponseHandler<ClaimsIdentity>
    {
        private readonly IRelayStateHandler _relayStateHandler;
        private readonly IFederationPartyContextBuilder _federationPartyContextBuilder;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserClaimsProvider<SecurityToken> _identityProvider;
        public ResponseHandler(IRelayStateHandler relayStateHandler, IFederationPartyContextBuilder federationPartyContextBuilder, ITokenHandler tokenHandler, IUserClaimsProvider<SecurityToken> identityProvider)
        {
            this._relayStateHandler = relayStateHandler;
            this._federationPartyContextBuilder = federationPartyContextBuilder;
            this._tokenHandler = tokenHandler;
            this._identityProvider = identityProvider;
        }
        public async Task<ClaimsIdentity> Handle(HttpPostResponseContext context)
        {
            var elements = context.Form;
            var responseBase64 = elements["SAMLResponse"];
            var responseBytes = Convert.FromBase64String(responseBase64);
            var responseText = Encoding.UTF8.GetString(responseBytes);
            
            var relayState = await this._relayStateHandler.GetRelayStateFromFormData(elements);
            //this.SaveTemp(responseText);
            var xmlReader = XmlReader.Create(new StringReader(responseText));
            this.ValidateResponseSuccess(xmlReader);
            var token = _tokenHandler.ReadToken(xmlReader, relayState.ToString());
            var validator = this._tokenHandler as ITokenValidator;
            var validationResult = new List<ValidationResult>();
            var isValid = validator.Validate(token, validationResult, relayState.ToString());

            var identity = await this._identityProvider.GenerateUserIdentitiesAsync(token, new[] { context.AuthenticationMethod });
            return identity[context.AuthenticationMethod];
           
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
        //private void SaveTemp(string responseText)
        //{
        //    try
        //    {
        //        var path = @"D:\Dan\Software\Apira\Assertions\";
        //        var now = DateTimeOffset.Now;
        //        var tag = String.Format("{0}{1}{2}{3}{4}", now.Year, now.Month, now.Day, now.Hour, now.Minute);
        //        var writer = XmlWriter.Create(String.Format("{0}{1}{2}", path, tag, ".xml"));
        //        var el = new XmlDocument();
        //        el.Load(new StringReader(responseText));
        //        el.DocumentElement.WriteTo(writer);
        //        writer.Flush();
        //        writer.Dispose();
        //    }
        //    catch(Exception)
        //    {
        //        //ignore
        //    }
        //}
    }
}