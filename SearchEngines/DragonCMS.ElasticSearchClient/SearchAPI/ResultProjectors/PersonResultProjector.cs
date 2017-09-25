using System.Linq;
using Nest;

namespace ElasticSearchClient.SearchAPI.ResultProjectors
{
    internal class PersonResultProjector : ResultProjector<EsPersonSearch, QmPersonSearchResult>
    {
        public override SearchResult<QmPersonSearchResult> GetResult(ISearchResponse<EsPersonSearch> response)
        {
            //project here and handle stats
            var results = response.Documents.Select(x => new QmPersonSearchResult
            {
                Id = x.Id,
                PersonName = x.PersonName
            });
            var result = new SearchResult<QmPersonSearchResult>(results);
            base.BuildStats(response, result);
            return result;
        }
    }
}