using Nest;
using SearchEngine.Infrastructure;

namespace ElasticSearchClient.SearchAPI.ResultProjectors
{
    internal abstract class ResultProjector<TModel, TResult> : IResultProjector<TModel, TResult>
        where TModel : class 
        where TResult : class
    {
        public abstract SearchResult<TResult> GetResult(ISearchResponse<TModel> response);
        protected void BuildStats(ISearchResponse<TModel> response, SearchResult<TResult> result)
        {
            result.RecordsReturned = response.Documents != null ? response.Documents.Count : 0;
            result.TotalCount = response.Total;
            result.RetrievalTime = response.Took;
        }
    }
}