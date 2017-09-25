using Nest;

namespace ElasticSearchClient.IndexAPI.PersonIndexMappers
{
    internal class PersonOrganisationMapper : PropertyMapper<EsPersonSearch>
    {
        protected override TypeMappingDescriptor<EsPersonSearch> MapPropertyInternal(TypeMappingDescriptor<EsPersonSearch> descriptor)
        {
            return descriptor.Properties(x => x.Nested<EsOrganisationSearch>(n => n.Name(p => p.Organisations))); ;
        }
    }
}