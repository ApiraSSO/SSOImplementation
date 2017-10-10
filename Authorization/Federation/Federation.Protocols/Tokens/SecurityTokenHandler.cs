using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Kernel.Authentication.Claims;
using Kernel.Federation.Tokens;

namespace Federation.Protocols.Tokens
{
    internal class SecurityTokenHandler : ITokenHandler
    {
        private readonly ITokenSerialiser _tokenSerialiser;
        private readonly ITokenValidator _tokenValidator;
        private readonly IUserClaimsProvider<Saml2SecurityToken> _identityProvider;

        public SecurityTokenHandler(ITokenSerialiser tokenSerialiser, ITokenValidator tokenValidator, IUserClaimsProvider<Saml2SecurityToken> identityProvider)
        {
            this._tokenSerialiser = tokenSerialiser;
            this._tokenValidator = tokenValidator;
            this._identityProvider = identityProvider;
        }
        
        public async Task<TokenHandlingResponse> HandleToken(HandleTokenContext context)
        {
            var relayState = context.RelayState as IDictionary<string, object>;
            if (relayState == null)
                throw new InvalidOperationException(String.Format("Expected relay state type of: {0}, but it was: {1}", typeof(IDictionary<string, object>).Name, context.RelayState.GetType().Name));
            var partnerId = relayState["federationPartyId"].ToString();
            ClaimsIdentity identity = null;
            var token = this._tokenSerialiser.DeserialiseToken(context.Token, partnerId);
            var validationResult = new List<ValidationResult>();
            var isValid = this._tokenValidator.Validate(token, validationResult, partnerId);

            if (isValid)
            {
                var identities = await this._identityProvider.GenerateUserIdentitiesAsync((Saml2SecurityToken)token, new[] { context.AuthenticationMethod });
                identity = identities[context.AuthenticationMethod];
            }

            return new TokenHandlingResponse(token, identity, validationResult);
        }
    }
}