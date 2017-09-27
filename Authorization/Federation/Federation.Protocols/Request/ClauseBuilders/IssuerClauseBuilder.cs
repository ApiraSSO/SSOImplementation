using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

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