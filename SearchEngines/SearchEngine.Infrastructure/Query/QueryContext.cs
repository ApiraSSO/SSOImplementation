using System.Collections.Generic;

namespace SearchEngine.Infrastructure.Query
{
    public class QueryContext
    {
        public QueryContext()
        {
            this.PageContext = new PageContext();
            this.SearchFields = new List<FieldDescriptor>();
            this.SortContext = new SortContext();
        }
        public PageContext PageContext { get; set; }

        public IndexContext IndexContext { get; set; }

        public ICollection<FieldDescriptor> SearchFields { get; set; }

        public SortContext SortContext { get; set; }

        public string Operator { get; set; }
    }
}