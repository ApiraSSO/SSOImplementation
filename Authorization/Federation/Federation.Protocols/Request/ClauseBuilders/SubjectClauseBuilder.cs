using Kernel.Federation.MetaData.Configuration.EntityDescriptors;

namespace Federation.Protocols.Request.ClauseBuilders
{
    internal class SubjectClauseBuilder : ClauseBuilder
    {
        protected override void BuildInternal(AuthnRequest request, EntityDesriptorConfiguration entityDescriptor)
        {
            //ToDo: Subject
            //request.NameIdPolicy = new NameIdPolicy();
        }
    }
}