﻿using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class NameIdPolicyClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            //ToDo: NameId policy
            //request.NameIdPolicy = new NameIdPolicy();
        }
    }
}