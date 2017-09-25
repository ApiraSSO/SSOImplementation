using Kernel.Initialisation;
using Nest;
using SearchEngine.Infrastructure;

namespace ElasticSearchClient.SearchAPI.ResultProjectors
{
    internal interface IResultProjector<TModel, TResult> : IAutoRegisterAsTransient
        where TModel : class
        where TResult : class
    {
        SearchResult<TResult> GetResult(ISearchResponse<TModel> response);
    }
}