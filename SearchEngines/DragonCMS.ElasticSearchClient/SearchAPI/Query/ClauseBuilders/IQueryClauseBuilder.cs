using System;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI.Query.ClauseBuilders
{
    public interface IQueryClauseBuilder<TField> where TField : FieldBase
    {
        Func<QueryContainerDescriptor<T>, QueryContainer> BuildQueryClause<T>(TField fieldContext) where T : class;
    }
}