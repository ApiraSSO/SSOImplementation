using System.Collections.Generic;

namespace SearchEngine.Infrastructure
{
    public interface ISearchStat
    {
        long TotalCount { get; set; }
        uint Page { get; set; }
        long RecordsReturned { get; set; }
    }
    public class SearchStats : ISearchStat
    {
        public long TotalCount { get; set; }
        public uint Page { get; set; }
        public long RecordsReturned { get; set; }
        public long RetrievalTime { get; set; }
    }
    public class SearchResult<TModel> : SearchStats
    {
        public SearchResult() : this(new List<TModel>())
        {
        }

        public SearchResult(IEnumerable<TModel> entities)
        {
            this.Entities = entities;
        }
        public IEnumerable<TModel> Entities { get; private set; }
    }
}