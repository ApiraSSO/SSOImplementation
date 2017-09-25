using System.Linq;
using Nest;
using SearchEngine.Infrastructure;

namespace ElasticSearchClient.SearchAPI.ResultProjectors
{
    internal class IdsResultProjector : ResultProjector<EsPersonSearch, QmSearchResult>
    {
        public override SearchResult<QmSearchResult> GetResult(ISearchResponse<EsPersonSearch> response)
        {
            var results = response.Documents.Select(x => new QmSearchResult
            {
                Id = x.Id,
            });
            var result = new SearchResult<QmSearchResult>(results);
            base.BuildStats(response, result);
            return result;
        }
    }
}