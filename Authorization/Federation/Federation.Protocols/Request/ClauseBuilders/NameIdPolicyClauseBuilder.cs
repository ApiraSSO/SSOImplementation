﻿using System;
using Kernel.Federation.FederationPartner;
using Shared.Federtion.Constants;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class NameIdPolicyClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, AuthnRequestConfiguration configuration)
        {
            request.NameIdPolicy = new NameIdPolicy
            {
                AllowCreate = configuration.AllowCreateNameIdPolicy,
                Format = String.IsNullOrWhiteSpace(configuration.NameIdPolicyFormat) ? 
                configuration.EncryptNameId ? 
                NameIdentifierFormats.Encrypted : NameIdentifierFormats.Transient : configuration.NameIdPolicyFormat
            };
        }
    }
}