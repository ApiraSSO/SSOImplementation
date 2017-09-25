using System;

namespace SearchEngine.Infrastructure
{
    public class IndexContext<TIndex> : IndexContext
    {
        private string _indexName;

        public IndexContext(string indexName = null) : base(typeof(TIndex), indexName)
        {
        }
    }
}