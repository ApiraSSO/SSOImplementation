namespace SearchEngine.Infrastructure.Query
{
    public class SearchContext
    {
        public SearchContext(TypeContext typeContext, PagedSearchRequest request, QueryContext queryContext, QueryResultContext queryResultContext)
        {
            this.TargetTypeContext = typeContext;
            this.Request = request;
            this.QueryContext = queryContext;
            this.QueryResultContext = queryResultContext;
        }
        public TypeContext TargetTypeContext { get; }
        public PagedSearchRequest Request { get; }
        public QueryContext QueryContext { get; }
        public QueryResultContext QueryResultContext { get; }
    }
}