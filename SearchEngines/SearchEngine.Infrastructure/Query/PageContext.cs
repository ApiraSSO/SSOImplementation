namespace SearchEngine.Infrastructure.Query
{
    public class PageContext
    {
        public PageContext()
        {
            this.Page = 0;
            this.PageSize = 10;
        }
        public uint Page { get; set; }
        public uint PageSize { get; set; }
    }
}