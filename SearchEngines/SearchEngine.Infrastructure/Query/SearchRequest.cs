using System.Collections.Generic;

namespace SearchEngine.Infrastructure.Query
{
    public class PagedSearchRequest
    {
        public string Text { get; set; }
        public int PageNumber { get; set; }
        public int Take { get; set; }
        public int AggregateType { get; set; }
        public int AggregateSubType { get; set; }
        public IEnumerable<int> Products { get; set; }
    }
}