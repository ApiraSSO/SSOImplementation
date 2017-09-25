namespace OAuthAuthorisationService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kernel.Authentication;
    using Kernel.Authentication.Services;
    using Kernel.DependancyResolver;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    /// <summary>
    /// OAuth provider
    /// </summary>
    internal class UserOAuthProvider : OAuthAuthorizationServerProvider
    {
		private readonly IDependencyResolver dependencyResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserOAuthProvider"/> class.
		/// </summary>
		/// <param name="dependencyResolver">The authentication service.</param>
		public UserOAuthProvider(IDependencyResolver dependencyResolver)
		{
			this.dependencyResolver = dependencyResolver;
		}

		/// <summary>
		/// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
		/// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and
		/// optional "refresh_token". If the web application supports the
		/// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
		/// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
		/// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
		/// The default behavior is to reject this grant type.
		/// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
		/// </summary>
		/// <param name="context">The context of the event carries information in and results out.</param>
		/// <returns>
		/// Task to enable asynchronous execution
		/// </returns>
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var authenticationService = this.dependencyResolver.Resolve<IIdentityProviderService>();
            var psw = context.Password;
			var result = await authenticationService.AuthenticateUser(new AuthenticationTypesContext(context.UserName, string.Empty, ref psw, new[] { context.Options.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType }));

			switch (result.Result)
			{
				case AuthenticationResults.Success:
					{
                        AuthenticationProperties properties = this.CreateProperties(context.UserName);
                        var identity = result.Identities[context.Options.AuthenticationType];
                        var cookiesIdentity = result.Identities[CookieAuthenticationDefaults.AuthenticationType];
                        AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                        context.Validated(ticket);
                        context.Request.Context.Authentication.SignIn(cookiesIdentity);

                        break;
					}
				case AuthenticationResults.FailInvalidCredentials:
				case AuthenticationResults.FailInvalidUsername:
				case AuthenticationResults.FailInvalidPassword:
					context.SetError("invalid_grant", "Uncorrect username or password");
					break;
				default:
					context.SetError("invalid_grant", "Unexpected sign in result");
					break;
			}
		}

		 /// <summary>
		 /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
		 /// present on the request. If the web application accepts Basic authentication credentials,
		 /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
		 /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
		 /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
		 /// If context.Validated is not called the request will not proceed further.
		 /// </summary>
		 /// <param name="context">The context of the event carries information in and results out.</param>
		 /// <returns>
		 /// Task to enable asynchronous execution
		 /// </returns>
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		 {
			 // Resource owner password credentials does not provide a client ID.
			 if (context.ClientId == null)
			 {
				 context.Validated();
			 }

			 return Task.FromResult<object>(null);
		 }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            return base.ValidateClientRedirectUri(context);
        }

        private  AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}