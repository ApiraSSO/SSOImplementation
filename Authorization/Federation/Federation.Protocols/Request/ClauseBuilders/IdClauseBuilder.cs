using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class IdClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            request.Id = entityDescriptor.Id;
        }
    }
}