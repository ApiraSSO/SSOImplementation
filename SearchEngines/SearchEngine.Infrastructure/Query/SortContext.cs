using System.Collections.Generic;

namespace SearchEngine.Infrastructure.Query
{
    public class SortContext
    {
        public SortContext()
        {
            this.Fields = new List<SortField>();
        }
        public ICollection<SortField> Fields { get; }
    }
}