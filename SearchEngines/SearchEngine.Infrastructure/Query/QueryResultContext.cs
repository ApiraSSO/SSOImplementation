using System;

namespace SearchEngine.Infrastructure.Query
{
    public class QueryResultContext
    {
        public QueryResultContext(Type serviceType)
        {
            this.ResultServiceType = serviceType;
        }
        public SearchResult<QmSearchResult> SearchResult { get; private set; }
        public Type ResultServiceType { get; }

        public void AssignResult(SearchResult<QmSearchResult> searchResul)
        {
            this.SearchResult = searchResul;
        }
    }
}