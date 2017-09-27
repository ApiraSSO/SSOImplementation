using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using System.Linq;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class AssertionConsumerServiceClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            var defaultEndpoint = entityDescriptor.SPSSODescriptors.SelectMany(x => x.AssertionConsumerServices)
                .Single(x => x.IsDefault.GetValueOrDefault());
            request.AssertionConsumerServiceIndex = (ushort)defaultEndpoint.Index;
        }
    }
}