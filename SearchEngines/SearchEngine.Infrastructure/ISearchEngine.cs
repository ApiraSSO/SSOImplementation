using System;
using System.Threading.Tasks;
using SearchEngine.Infrastructure.Query;

namespace SearchEngine.Infrastructure
{
    public interface ISearchEngine
    {
        TResult SearchById<TModel, TResult>(Guid id)
            where TModel : class
            where TResult : class;

        Task<SearchResult<QmSearchResult>> Search(QueryContext context);
        Task<SearchResult<TResult>> Search<TModel, TResult>(QueryContext context)
            where TModel : class
            where TResult : class;
    }
}