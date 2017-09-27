using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class AudienceRestrictionClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            var audienceRestriction = new AudienceRestriction();
            audienceRestriction.Audience.Add(entityDescriptor.EntityId);
            request.Conditions.Items.Add(audienceRestriction);
        }
    }
}