using DragonCMS.Common.Dependencies;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClientTests.MockData;
using Nest;

namespace DragonCMS.ElasticSearchClientTests.IndexMappers
{
    internal class TestIndexMapper : IndexMapper<ParentTestClass>
    {
        public TestIndexMapper(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            
        }
        public override CreateIndexDescriptor Map(CreateIndexDescriptor descriptor)
        {
            descriptor.Mappings(m => m.Map<ParentTestClass>(x =>
            x.Properties(p => p.Nested<ChildClass>(nested => nested.Name(parent => parent.Children)))));
            return descriptor;
        }
    }
}