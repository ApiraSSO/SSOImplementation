using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Directory.Messages.Models.V1;
using Nest;

namespace DragonCMS.ElasticSearchClient.IndexAPI.PersonIndexMappers
{
    internal class PersonNameMapper : PropertyMapper<EsPersonSearch>
    {
        protected override TypeMappingDescriptor<EsPersonSearch> MapPropertyInternal(TypeMappingDescriptor<EsPersonSearch> descriptor)
        {
            return descriptor.Properties(x => 
            x.Object<PersonName>(n => n.Name(p => p.PersonName)
            .Properties(pr => 
            pr.Keyword(des => des.Name(n2 => n2.FirstName))
            .Keyword(des => des.Name(n3 => n3.LastName)))));
        }
    }
}