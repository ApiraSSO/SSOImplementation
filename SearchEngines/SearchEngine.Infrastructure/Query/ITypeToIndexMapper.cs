
using Kernel.Initialisation;

namespace SearchEngine.Infrastructure.Query
{
    public interface ITypeToIndexMapper : IAutoRegisterAsTransient
    {
        QueryContext BuildQueryContext(PagedSearchRequest searchRequest);
        void BuildIndexContext(QueryContext queryContext);
        void RegisterMapper(ITypeToIndexMapperManager typeToIndexMapperManager);
    }
}