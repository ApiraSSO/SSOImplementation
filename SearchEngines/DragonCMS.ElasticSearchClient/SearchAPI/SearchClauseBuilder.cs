using System;
using System.Linq.Expressions;
using ElasticSearchClient.IndexAPI;
using ElasticSearchClient.SearchAPI.Query;
using Nest;
using SearchEngine.Infrastructure.Query;

namespace ElasticSearchClient.SearchAPI
{
    internal class SearchClauseBuilder<T> : ISearchClauseBuilder<T> where T : class
    {
        BoolQueryBulder<T> _boolTermQueryBulder;
        SortClauseBuilder<T> _sortClauseBuilder;
        private readonly IIndexManager _indexManager;
        public SearchClauseBuilder(BoolQueryBulder<T> boolTermQueryBulder, SortClauseBuilder<T> sortClauseBuilder, IIndexManager indexManager)
        {
            this._boolTermQueryBulder = boolTermQueryBulder;
            this._sortClauseBuilder = sortClauseBuilder;
            this._indexManager = indexManager;
        }
        Expression<Func<ElasticClient, ISearchResponse<T>>> ISearchClauseBuilder<T>.BuildSearchClause(QueryContext context)
        {
            var query = this._boolTermQueryBulder.BuildQuery(context);
            var sortClause = this._sortClauseBuilder.BuildSortClause(context.SortContext);
            var index = this._indexManager.BuildIndexName(context.IndexContext);
            return c => c.Search<T>(s => 
            s.Index(index)
            .Query(query)
            .Sort(sortClause)
            .Size((int)context.PageContext.PageSize)
            .From((int)context.PageContext.Page * (int)context.PageContext.PageSize));
        }
    }
}