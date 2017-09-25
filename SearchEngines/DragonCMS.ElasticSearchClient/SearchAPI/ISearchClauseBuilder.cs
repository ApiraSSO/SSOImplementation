using System;
using System.Linq.Expressions;
using Kernel.Initialisation;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI
{
    public interface ISearchClauseBuilder<T> : IAutoRegisterAsTransient where T : class
    {
        Expression<Func<ElasticClient, ISearchResponse<T>>> BuildSearchClause(QueryContext context);
    }
}