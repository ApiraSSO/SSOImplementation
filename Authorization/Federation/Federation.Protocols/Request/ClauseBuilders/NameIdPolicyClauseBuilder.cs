using System;
using System.Linq;
using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class NameIdPolicyClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            var format = this.ResolveFormat(configuration);
            request.NameIdPolicy = new NameIdPolicy
            {
                AllowCreate = configuration.AllowCreateNameIdPolicy,
                Format = format
            };
        }

        private string ResolveFormat(AuthnRequestConfiguration configuration)
        {
            var format = NameIdentifierFormats.Unspecified;
            if (configuration.EncryptNameId || configuration.DefaultNameIdFormat == new Uri(NameIdentifierFormats.Encrypted))
                return NameIdentifierFormats.Encrypted;
            if(configuration.DefaultNameIdFormat != null)
            {
                var defaultNameId = configuration.SupportedNameIdentifierFormats.FirstOrDefault(x => x == configuration.DefaultNameIdFormat);
                if (defaultNameId != null)
                    format = defaultNameId.AbsoluteUri;
            }

            return format;
        }
    }
}