using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Shared.Federtion.Models;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class IssuerClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            request.Issuer = new NameId { Value = entityDescriptor.EntityId };
        }
    }
}