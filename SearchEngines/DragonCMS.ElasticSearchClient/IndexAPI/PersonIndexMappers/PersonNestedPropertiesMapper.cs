using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Common.Dependencies;

namespace DragonCMS.ElasticSearchClient.IndexAPI.PersonIndexMappers
{
    internal class PersonNestedPropertiesMapper : IndexMapper<EsPersonSearch>
    {
        public PersonNestedPropertiesMapper(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            this.Mappers = dependencyResolver.ResolveAll<PropertyMapper<EsPersonSearch>>();
        }
    }
}